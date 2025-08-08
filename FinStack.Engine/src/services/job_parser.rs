use std::str::FromStr;

use chrono::{DateTime, Utc};
use serde::de::DeserializeOwned;
use crate::{jobs::ImportFileJob, models::JobCode, traits::ScheduledJob, utils::ptr_to_string};

pub struct JobResult {
    pub success: bool,
    pub elapsed: i64,
    pub start_time: DateTime<Utc>,
    pub finish_time: DateTime<Utc>,
    pub message: String,
}

pub struct JobParser;

impl JobParser {
    pub fn new() -> Self {
        JobParser
    }

    pub fn create_job_from_code(code: JobCode, json: String) -> Box<dyn ScheduledJob> {
        match code {
            JobCode::ImportFile => serde_json::from_str(&json).expect("ERROR")
        }
    }



    pub fn parse(
        &self,
        job_code_ptr: *const i8,
        job_body_ptr: *const i8,
    ) -> Result<impl ScheduledJob, String> {

        let job_code_str = ptr_to_string(job_code_ptr).ok_or("Invalid job code pointer")?;
        let job_body_str = ptr_to_string(job_body_ptr).ok_or("Invalid job body pointer")?;

        let job_code = JobCode::from_str(&job_code_str)
            .map_err(|_| "Failed to parse JobCode".to_string())?;

        serde_json::from_str(&job_body_str)
        .expect("QQQQQQ")?
    }
}
