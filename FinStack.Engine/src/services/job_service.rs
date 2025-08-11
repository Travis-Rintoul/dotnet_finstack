use chrono::Utc;
use once_cell::sync::Lazy;
use tokio::runtime::Runtime;
use uuid::Uuid;

use crate::{db::{DbContext, JobsRepository, Repository}, models::JobDto, services::job_runner::JobRunner, traits::ScheduledJob};

static TOKIO_RUNTIME: Lazy<Runtime> = Lazy::new(|| {
    Runtime::new().expect("Failed to create Tokio runtime")
});

pub struct JobService;

impl JobService {

    pub fn new() -> Self {
        JobService
    }

    pub fn schedule_and_run(&self, job: Box<dyn ScheduledJob>) -> Result<(), String> {
        let validate = job.validate();

        if let Err(error) = &validate {
            return Err(error.clone());
        }

        TOKIO_RUNTIME.spawn(async move {

            let Ok(ctx) = DbContext::connect().await else {
                return Err("Unable to connect to DB");
            };

            let dto: JobDto;
            let job_code = job.code().to_string(); 
            let jobs = JobsRepository::new(ctx);

            if let Err(error) = validate {
                log::error!("Failed Validation: {error}");
                dto = JobDto {
                    id: -1,
                    guid: Uuid::new_v4(),
                    job_code,
                    elapsed: 0,
                    success: false,
                    start_time: Utc::now(),
                    finish_time: Utc::now(),
                    message: error
                };
            } else {
                let runner = JobRunner::new();
                let result = runner.execute(job).await;

                dto = JobDto {
                    id: -1,
                    guid: Uuid::new_v4(),
                    job_code,
                    elapsed: result.elapsed,
                    success: result.success,
                    start_time: result.start_time,
                    finish_time: result.finish_time,
                    message: "".to_string()
                };
            }

            if let Err(e) = jobs.create(dto).await {
                log::error!("Failed to create job record: {}", e);
            }

            Ok(())
        });

        Ok(())
    }
}