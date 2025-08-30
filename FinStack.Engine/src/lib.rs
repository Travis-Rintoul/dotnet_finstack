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

pub unsafe extern "C" fn configure(pointer: *const u8, len: usize) -> i32 {
    let slice = unsafe { std::slice::from_raw_parts(pointer, len) };
    match serde_json::from_slice::<Config>(slice) {
        Ok(config) => {
            if config.mode != "Test" {
                config.connection_string.contains("finstack_db"); // TODO: replace this
            }
            CONFIG.set(config).map(|_| 0).unwrap_or(1)
        }
        Err(_) => 3,
    }
}

#[unsafe(no_mangle)]
pub unsafe extern "C" fn schedule_job(
    command_code_ptr: *const i8,
    command_body_ptr: *const i8,
) -> i32 {
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
        Ok(_) => 1,
        Err(code) => code,
    }
}
