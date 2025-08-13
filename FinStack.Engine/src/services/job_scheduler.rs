use std::{any::Any, sync::Arc};

use chrono::Utc;
use once_cell::sync::Lazy;
use tokio::runtime::Runtime;

use crate::{
    JobGuid,
    commands::CreateJobCommand,
    db::{DbContext, JobsRepository, JobsRepositoryTrait, RepositoryFactory},
    models::JobDto,
    services::{
        command_router::{CommandDependencies, CommandRouter},
        job_runner::JobResult,
    },
};

static TOKIO_RUNTIME: Lazy<Runtime> =
    Lazy::new(|| Runtime::new().expect("Failed to create Tokio runtime"));

pub fn schedule_job_and_run(
    command_name: String,
    command: Box<dyn Any + Send + Sync>
) -> JobGuid {
    let job_guid = uuid::Uuid::new_v4();

    TOKIO_RUNTIME.spawn(async move {
        let Ok(ctx) = DbContext::connect().await else {
            return Err("Unable to connect to DB");
        };

        let db_context = Arc::new(ctx);
        let repo_factory = RepositoryFactory::new(Arc::clone(&db_context));

        let deps = Arc::new(CommandDependencies::new(
            Arc::clone(&db_context),
            Box::new(repo_factory),
        ))
        ;
        let router = CommandRouter::new(deps);

        //let command_name: String = command.name();
        let start_time = Utc::now();
        log::info!(
            "Job Started {} ({:?}) at {}",
            command_name,
            job_guid,
            start_time
        );

        let command_result = router.send(command).await;

        let finish_time = Utc::now();
        let elapsed = finish_time - start_time;

        log::info!(
            "Job Finished {} ({:?}) at {} elapsed {:.3}",
            command_name,
            job_guid,
            finish_time,
            elapsed
        );

        let elapsed_us: i64 = elapsed.num_microseconds().unwrap_or(0);

        let (success, message) = match command_result {
            Ok(_) => (true, String::new()),
            Err(e) => (false, e.to_string()),
        };

        let create_job_cmd = Box::new(CreateJobCommand {
            job_guid,
            job_code: command_name,
            elapsed: elapsed_us,
            success: success,
            start_time,
            finish_time,
            message,
        });

        let result = router.send(create_job_cmd).await;

        if result.is_err() {
            log::error!("ERROR CREATING JOB");
        }

        Ok(())
    });

    JobGuid(*job_guid.as_bytes())
}
