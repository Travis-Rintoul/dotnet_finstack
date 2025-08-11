use std::{error::Error, time::Duration};

use async_trait::async_trait;
use chrono::Utc;
use serde::Deserialize;
use tokio::time::sleep;
use uuid::Uuid;

use crate::{
    db::{DbContext, JobsRepository, Repository},
    models::{JobCode, JobDto},
    traits::{JobContext, ScheduledJob},
};

#[derive(Deserialize)]
pub struct ImportFileJob {
    file_name: String,
}

#[cfg(test)]
impl ImportFileJob {
    pub fn new(file_name: String) -> Self {
        Self { file_name }
    }
}

#[async_trait]
impl ScheduledJob for ImportFileJob {
    fn code(&self) -> JobCode {
        JobCode::ImportFile
    }

    fn validate(&self) -> Result<(), String> {
        if self.file_name.is_empty() {
            return Err("File name required".to_string());
        }

        Ok(())
    }

    async fn prepare(&self) -> Result<JobContext, Box<dyn Error + Send + Sync>>
    {
        
    }

    async fn execute(&self) -> Result<(), Box<dyn Error + Send + Sync>> {
        log::info!("Executing ImportFileJob!");

        let ctx = DbContext::connect().await?;
        let repo = JobsRepository::new(ctx);

        repo.create(JobDto {
            id: -1,
            guid: Uuid::new_v4(),
            job_code: "Test".to_string(),
            elapsed: 0,
            success: false,
            start_time: Utc::now(),
            finish_time: Utc::now(),
            message: "".to_string()
        })
        .await?;

        Ok(())
    }
}

#[cfg(test)]
mod tests {

    use super::*;

    #[test]
    fn should_validate_correctly() {
        let job = ImportFileJob::new("test.json".to_string());
        assert!(job.validate().is_ok());
    }

    #[test]
    fn should_validate_failure() {
        let job = ImportFileJob::new("".to_string());
        assert!(job.validate().is_err());
    }

    #[tokio::test]
    async fn should_execute_correctly() {
        let job = ImportFileJob {
            file_name: "test.txt".into(),
        };
        let start = std::time::Instant::now();
        let result = job.execute().await;

        assert!(result.is_ok());

    }
}
