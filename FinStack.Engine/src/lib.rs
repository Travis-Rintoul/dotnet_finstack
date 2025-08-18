use std::{any::Any, ffi::c_char, ptr::copy_nonoverlapping};

use log::info;

use crate::{services::{command_parser::CommandParser, job_scheduler::schedule_job_and_run}, utils::{ptr_to_string, setup_logger}};

mod commands;
mod config;
mod db;
mod error;
mod jobs;
mod models;
mod services;
mod traits;
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
pub unsafe extern "C" fn schedule_job(command_code_ptr: *const i8, command_body_ptr: *const i8) -> i32 {
    fn inner(command_code_ptr: *const i8, command_body_ptr: *const i8) -> Result<JobGuid, i32> {
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


        let parser = CommandParser::new();

        let command: Box<dyn Any + Send + Sync> = match parser.parse(&command_code, &command_body) {
            Ok(cmd) => cmd,
            Err(error) => {
                log::error!("{error}");
                return Err(-4);
            }
        };

        Ok(schedule_job_and_run(command_code, command))
    }

    match inner(command_code_ptr, command_body_ptr) {
        Ok(guid) => 1,
        Err(code) => code,
    }
}
