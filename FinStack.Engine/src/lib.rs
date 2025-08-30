use std::ptr;

use serde_json::from_str;

use crate::{
    config::{Config, CONFIG}, services::{command_and_query_service::Command, job_scheduler::schedule_job_and_run}, utils::{ptr_to_string, setup_logger}
};
mod config;
mod db;
mod error;
mod jobs;
mod models;
mod services;
mod utils;

#[repr(C)]
pub struct JobGuid([u8; 16]);

impl JobGuid {
    pub fn as_bytes(&self) -> &[u8] {
        &self.0
    }
}

#[repr(C)]
pub struct JobResult {
    pub error_code: i32,
    pub guid: [u8; 16],
}

#[unsafe(no_mangle)]

pub extern "C" fn configure(pointer: *const u8, len: usize) -> i32 {
    if pointer.is_null() || len == 0 {
        eprintln!("configure: null pointer or zero length");
        return 2;
    }

    let slice = unsafe { std::slice::from_raw_parts(pointer, len) };
    let config: Config = match serde_json::from_slice(slice) {
        Ok(cfg) => cfg,
        Err(e) => {
            eprintln!("configure: invalid JSON: {e}");
            return 3;
        }
    };

    if let Err(_already_set) = CONFIG.set(config) {
        eprintln!("configure: CONFIG already set (continuing)");
    }

    match setup_logger() {
        Ok(path) => {
            eprintln!("Rust logs -> {:?}", path);
            0
        }
        Err(e) => {
            eprintln!("Logger initialization failed: {e}");
            -1
        }
    }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn schedule_job(
    command_code_ptr: *const i8,
    command_body_ptr: *const i8,
    guid_out_ptr: *mut u8,
) -> i32 {

    fn to_dotnet_guid_bytes(rfc: &[u8; 16]) -> [u8; 16] {
        let mut b = *rfc;
        b[0..4].reverse();
        b[4..6].reverse();
        b[6..8].reverse();
        b
    }

    fn inner(command_code_ptr: *const i8, command_body_ptr: *const i8) -> Result<JobGuid, i32> {

        if CONFIG.get().is_none() {
            return Err(9999);
        }

        if let Err(e) = setup_logger() {
            eprintln!("Logger initialization failed: {}", e);
            return Err(-1);
        }

        let Some(command_code) = ptr_to_string(command_code_ptr) else {
            return Err(-2);
        };

        let Some(command_body) = ptr_to_string(command_body_ptr) else {
            return Err(-3);
        };

        log::info!("{command_code} {command_body}");

        let command: Command = from_str(&command_body).map_err(|e| {
            log::error!("{e}");
            return -4;
        })?;

        log::info!("{}", command);

        Ok(schedule_job_and_run(command_code, command))
    }

    match inner(command_code_ptr, command_body_ptr) {
        Ok(guid) => {
            let dotnet = to_dotnet_guid_bytes(&guid.0);
            unsafe { ptr::copy_nonoverlapping(dotnet.as_ptr(), guid_out_ptr, 16) };
            0
        }
        Err(code) => {
            code
        }
    }
}
