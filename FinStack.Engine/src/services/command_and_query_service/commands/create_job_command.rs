use async_trait::async_trait;
use chrono::{DateTime, Utc};
use serde::Deserialize;
use std::{error::Error, sync::Arc};
use uuid::Uuid;

use crate::{
    models::JobDto, services::command_and_query_service::{traits::CommandTrait, CQRSDependencies},
};

#[derive(Debug, Deserialize, Clone)]
pub struct CreateJobCommand {
    pub job_guid: Uuid,
    pub job_code: String,
    pub elapsed: i64,
    pub success: bool,
    pub start_time: DateTime<Utc>,
    pub finish_time: DateTime<Utc>,
    pub message: String,
}

#[async_trait]
impl CommandTrait for CreateJobCommand {
    async fn handle(
        &self,
        services: Arc<CQRSDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        let Some(repo_factory) = &services.repository_factory else {
            log::error!("Unable to initialize repository");
            return Err("".into());
        };

        let jobs_repository = repo_factory.create_jobs_repository();
        let dto = JobDto {
            id: -1,
            guid: self.job_guid,
            job_code: self.job_code.clone(),
            elapsed: self.elapsed,
            success: self.success,
            start_time: self.start_time,
            finish_time: self.finish_time,
            message: self.message.clone(),
        };

        if let Err(error) = jobs_repository.create(dto).await {
            log::error!("{}", error);
            return Err(format!("{error}").into());
        }

        Ok(())
    }
}

#[cfg(test)]
mod tests {

    use crate::{db::MockRepositoryFactory, services::command_and_query_service::CQRSDispatcher};

    use super::*;
    use std::sync::Arc;

    #[tokio::test]
    async fn should_pass() {
        let factory = MockRepositoryFactory::new();

        factory
            .mock_jobs
            .create_method
            .set_behavior(|q| return Ok(q.guid));

        let dependencies = Arc::new(CQRSDependencies {
            db: None,
            repository_factory: Some(Box::new(factory)),
        });

        let dispatcher = CQRSDispatcher::new(dependencies);
        let command = CreateJobCommand {
            job_guid: Uuid::new_v4(),
            job_code: "import-file".to_string(),
            elapsed: 1,
            success: true,
            start_time: Utc::now(),
            finish_time: Utc::now(),
            message: "".to_string(),
        };

        let result = dispatcher.send_command(command).await;
        assert!(result.is_ok(), "Handler should return Ok result");
    }

    #[tokio::test]
    async fn should_fail() {
        let factory = MockRepositoryFactory::new();

        factory
            .mock_jobs
            .create_method
            .set_behavior(|_| return Err("ERROR".into()));

        let dependencies = Arc::new(CQRSDependencies {
            db: None,
            repository_factory: Some(Box::new(factory)),
        });

        let dispatcher = CQRSDispatcher::new(dependencies);
        let command = CreateJobCommand {
            job_guid: Uuid::new_v4(),
            job_code: "import-file".to_string(),
            elapsed: 1,
            success: true,
            start_time: Utc::now(),
            finish_time: Utc::now(),
            message: "".to_string(),
        };

        let result = dispatcher.send_command(command).await;
        assert!(result.is_err(), "Handler should return Err result");
    }
}
