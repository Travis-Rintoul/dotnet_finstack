use chrono::Utc;
use once_cell::sync::Lazy;
use tokio::runtime::Runtime;
use uuid::Uuid;

use crate::{
    db::{DbContext, JobsRepository, JobsRepositoryTrait, RepositoryFactory}, models::JobDto, services::{
        command_builder::CommandRequestBuilder,
        command_router::Command, job_runner::JobResult,
    }, JobGuid
};

static TOKIO_RUNTIME: Lazy<Runtime> =
    Lazy::new(|| Runtime::new().expect("Failed to create Tokio runtime"));

pub fn schedule_job_and_run(command: Command) -> JobGuid {

    let job_guid = uuid::Uuid::new_v4();

    TOKIO_RUNTIME.spawn(async move {
        let Ok(ctx) = DbContext::connect().await else {
            return Err("Unable to connect to DB");
        };

        let router = CommandRequestBuilder::new()
            .add_db_context(ctx)
            .add_repository_factory(Box::new(RepositoryFactory::new()))
            .build();

        let start_time = Utc::now();
        log::info!("Job Started {:?} at {}", command.into(), start_time);

        let command_result = router.send(command).await;

        let finish_time = Utc::now();
        let elapsed = finish_time - start_time;

        log::info!("Job Finished {:?} at {} elapsed {:.3}", command.into(), finish_time, elapsed);

        let elapsed_us: i64 = elapsed.num_microseconds().unwrap_or(0);

        let job_result = match command_result {
            Ok(_) => JobResult {
                success: true,
                elapsed: elapsed_us,
                start_time: start_time,
                finish_time: finish_time,
                message: "".to_string(),
            },
            Err(e) => JobResult {
                success: false,
                elapsed: elapsed_us,
                start_time: start_time,
                finish_time: finish_time,
                message: format!("{e}")
            },
        };

        let dto = JobDto {
            id: -1,
            guid: job_guid,
            job_code: todo!(),
            elapsed: todo!(),
            success: todo!(),
            start_time,
            finish_time,
            message: todo!(),
        };

        let repository = JobsRepository::new(ctx);



        repository.create(dto).await;


        Ok(())
    });

    JobGuid(*job_guid.as_bytes())

}
