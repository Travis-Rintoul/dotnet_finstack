mod commands;
mod config;
mod db;
mod error;
mod jobs;
mod models;
mod services;
mod traits;
mod utils;

use std::ffi::c_char;

use crate::{
    services::{command_parser::CommandParser, job_scheduler::schedule_job_and_run},
    utils::{ptr_to_string, setup_logger},
};

#[repr(C)]
pub struct JobGuid([u8; 16]);

#[unsafe(no_mangle)]
pub unsafe extern "C" fn schedule_job(
    command_name_ptr: *const i8,
    command_args_ptr: *const i8,
) -> *mut c_char {
    fn inner(command_name_ptr: *const i8, command_args_ptr: *const i8) -> Result<JobGuid, i32> {
        if let Err(e) = setup_logger() {
            eprintln!("Logger initialization failed: {}", e);
            return Err(-1);
        }

        let Some(command_name) = ptr_to_string(command_name_ptr) else {
            return Err(-2);
        };

        let Some(command_args) = ptr_to_string(command_args_ptr) else {
            return Err(-3);
        };

        let Ok(command) = CommandParser::parse(&command_name, &command_args) else {
            return Err(-4);
        };

        Ok(schedule_job_and_run(command))
    }

    match inner(command_name_ptr, command_args_ptr) {
        Ok(()) => 1,
        Err(code) => code,
    }
}
