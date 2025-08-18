use std::{any::Any, sync::Arc};

use async_trait::async_trait;
use chrono::{DateTime, Utc};
use uuid::Uuid;

use crate::{
    models::JobDto,
    services::commands_service::{CommandDependencies, CommandError, CommandHandler},
};

#[derive(Debug)]
pub struct CreateJobCommand {
    pub job_guid: Uuid,
    pub job_code: String,
    pub elapsed: i64,
    pub success: bool,
    pub start_time: DateTime<Utc>,
    pub finish_time: DateTime<Utc>,
    pub message: String,
}

pub struct CreateJobCommandHandler;

#[async_trait]
impl CommandHandler<CreateJobCommand> for CreateJobCommandHandler {
    async fn handle(
        &self,
        dependencies: Arc<CommandDependencies>,
        args: CreateJobCommand,
    ) -> Result<Box<dyn Any + Send>, CommandError> {
        log::info!("Running CreateJobCommand");

        let Some(repo_factory) = &dependencies.repository_factory else {
            log::error!("Unable to initialize repository");
            return Err("".into());
        };

        let jobs_repository = repo_factory.create_jobs_repository();

        let dto = JobDto {
            id: -1,
            guid: args.job_guid,
            job_code: args.job_code,
            elapsed: args.elapsed,
            success: args.success,
            start_time: args.start_time,
            finish_time: args.finish_time,
            message: args.message,
        };

        if let Err(error) = jobs_repository.create(dto).await {
            return Err(format!("{error}").into());
        }

        Ok(Box::new(()) as Box<dyn Any + Send>)
    }
}

#[cfg(test)]
mod tests {
    use crate::{db::MockRepositoryFactory, services::commands_service::CommandRouter};

    use super::*;
    use std::sync::Arc;

    #[tokio::test]
    async fn should_pass() {
        let factory = MockRepositoryFactory::new();

        factory
            .mock_jobs
            .create_method
            .set_behavior(|q| return Ok(q.guid));

        let dependencies = Arc::new(CommandDependencies {
            db: None,
            repository_factory: Some(Box::new(factory)),
        });

        let router = CommandRouter::new(dependencies);
        let command = CreateJobCommand {
            job_guid: Uuid::new_v4(),
            job_code: "import-file".to_string(),
            elapsed: 1,
            success: true,
            start_time: Utc::now(),
            finish_time: Utc::now(),
            message: "".to_string(),
        };

        let result = router.send(Box::new(command)).await;
        assert!(result.is_ok(), "Handler should return Ok result");
    }

    #[tokio::test]
    async fn should_fail() {
        let factory = MockRepositoryFactory::new();

        factory
            .mock_jobs
            .create_method
            .set_behavior(|_| return Err("ERROR".into()));

        let dependencies = Arc::new(CommandDependencies {
            db: None,
            repository_factory: Some(Box::new(factory)),
        });

        let router = CommandRouter::new(dependencies);
        let command = CreateJobCommand {
            job_guid: Uuid::new_v4(),
            job_code: "import-file".to_string(),
            elapsed: 1,
            success: true,
            start_time: Utc::now(),
            finish_time: Utc::now(),
            message: "".to_string(),
        };

        let result = router.send(Box::new(command)).await;
        assert!(result.is_err(), "Handler should return Err result");
    }
}
