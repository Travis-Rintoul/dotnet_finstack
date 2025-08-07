mod config;
mod db;
mod error;
mod models;
mod services;
mod traits;
mod utils;

use log::{error, info};
use crate::{services::job_service::JobService, utils::{ptr_to_string, setup_logger}};

#[unsafe(no_mangle)]
pub unsafe extern "C" fn schedule_job(job_code_ptr: *const i8, job_body_ptr: *const i8) -> i32 {
    fn inner(job_code_ptr: *const i8, job_body_ptr: *const i8) -> Result<(), i32> {

        if let Err(e) = setup_logger() {
            eprintln!("Logger initialization failed: {}", e);
            return Err(-999);
        }

        let Some(job_code_str) = ptr_to_string(job_code_ptr) else {
            error!("Unable to convert JobCode ptr to string.");
            return Err(-1);
        };

        let Some(job_body_str) = ptr_to_string(job_body_ptr) else {
            error!("Unable to convert JobBody ptr to string.");
            return Err(-2);
        };

        info!("schedule_job called with code: {job_code_str}, and body: {job_body_str}");

        let service = JobService::new();

        let job = service.parse(job_code_str, job_body_str)
            .inspect(|_| info!("Successfully parsed request..."))
            .map_err(|e| {
                error!("{e}");
                -4
            })?;

        job.validate()
            .inspect(|_| info!("Successfully validated request..."))
            .map_err(|_| {
                error!("Failed validation");
                -5
            })?;

        service.run(job);

        Ok(())
    }

    match inner(job_code_ptr, job_body_ptr) {
        Ok(()) => 0,
        Err(code) => code,
    }
}
