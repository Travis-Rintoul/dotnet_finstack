use std::{any::Any, error::Error, sync::Arc};

use async_trait::async_trait;
use chrono::{DateTime, Utc};
use uuid::Uuid;

use crate::{
    models::JobDto,
    services::command_router::{CommandDependencies, CommandError, CommandHandler},
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
            return Err("QQQQQ".to_string().into());
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

        if let Err(error) = jobs_repository.create(&dto).await {
            return Err(Box::<dyn Error + Send + Sync>::from("QQQQQ".to_string()));
        }

        Ok(Box::new(()) as Box<dyn Any + Send>)
    }
}
