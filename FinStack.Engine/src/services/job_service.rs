use chrono::Utc;
use once_cell::sync::Lazy;
use tokio::{runtime::Runtime, time::error};
use uuid::Uuid;

use crate::{db::create_job, models::JobDto, services::job_runner::JobRunner, traits::ScheduledJob, utils::ptr_to_string};

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
        let validate_copy = validate.clone();

        TOKIO_RUNTIME.spawn(async move {
            let job_code = job.code().to_string(); 
            let dto: JobDto;

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

            create_job(dto).await;
        });

        match validate_copy {
            Ok(_) => Ok(()),
            Err(error) => Err(error),
        }
    }
}