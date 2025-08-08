mod config;
mod db;
mod error;
mod jobs;
mod models;
mod services;
mod traits;
mod utils;

use log::{error, info};
use crate::{
    services::{job_parser::JobParser, job_service::JobService}, utils::setup_logger
};

#[unsafe(no_mangle)]
pub unsafe extern "C" fn schedule_job(job_code_ptr: *const i8, job_body_ptr: *const i8) -> i32 {
    fn inner(job_code_ptr: *const i8, job_body_ptr: *const i8) -> Result<(), i32> {
        if let Err(e) = setup_logger() {
            eprintln!("Logger initialization failed: {}", e);
            return Err(-1);
        }

        let service = JobService::new();        
        let parser = JobParser::new();

        let job = parser
            .parse(job_code_ptr, job_body_ptr)
            .inspect(|_| info!("Successfully parsed request..."))
            .map_err(|e| {
                error!("Parsing job: {e}");
                -4
            })?;

        match service.schedule_and_run(job) {
            Ok(_) => Ok(()),
            Err(_) => Err(-5) // Failed Validation
        }
    }

    match inner(job_code_ptr, job_body_ptr) {
        Ok(()) => 1,
        Err(code) => code,
    }
}
