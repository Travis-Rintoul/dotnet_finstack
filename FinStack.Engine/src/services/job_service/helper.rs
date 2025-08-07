use serde::de::DeserializeOwned;

use crate::{
    services::job_service::{JobCode, jobs::ImportFileJob},
    traits::ScheduledJob,
};

pub fn deserialize_job<T: ScheduledJob + DeserializeOwned + 'static>(
    json: &str,
) -> Result<Box<dyn ScheduledJob>, String> {
    serde_json::from_str::<T>(json)
        .map(|t| Box::new(t) as Box<dyn ScheduledJob>)
        .map_err(|e| e.to_string())
}

pub fn get_job_from_json(code: JobCode, json: &str) -> Result<Box<dyn ScheduledJob>, String> {
    match code {
        JobCode::ImportFile => deserialize_job::<ImportFileJob>(json),
    }
}
