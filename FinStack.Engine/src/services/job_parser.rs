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

    pub fn parse(
        &self,
        job_code_ptr: *const i8,
        job_body_ptr: *const i8,
    ) -> Result<Box<dyn ScheduledJob>, String> {

        let job_code_str = ptr_to_string(job_code_ptr).ok_or("Invalid job code pointer")?;
        let job_body_str = ptr_to_string(job_body_ptr).ok_or("Invalid job body pointer")?;

        match JobCode::from_str(&job_code_str) {
            Ok(code) => Self::get_job_from_json(code, &job_body_str),
            Err(error) => Err(error),
        }
    }

    fn get_job_from_json(code: JobCode, json: &str) -> Result<Box<dyn ScheduledJob>, String> {
        match code {
            JobCode::ImportFile => Self::deserialize_job::<ImportFileJob>(json),
        }
    }

    fn deserialize_job<T: ScheduledJob + DeserializeOwned + 'static>(
        json: &str,
    ) -> Result<Box<dyn ScheduledJob>, String> {
        serde_json::from_str::<T>(json)
            .map(|t| Box::new(t) as Box<dyn ScheduledJob>)
            .map_err(|e| e.to_string())
    }


}
