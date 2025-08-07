use std::str::FromStr;

use chrono::{DateTime, Utc};
use uuid::Uuid;

use crate::{
    db::create_job, models::JobDto, services::job_service::{get_job_from_json, JobCode}, traits::ScheduledJob
};

pub struct JobService;

impl JobService {
    pub fn new() -> Self {
        JobService
    }

    pub fn parse(
        &self,
        job_code_str: String,
        job_body_str: String,
    ) -> Result<Box<dyn ScheduledJob>, String> {
        let job_code = JobCode::from_str(&job_code_str)?;
        let job = get_job_from_json(job_code, &job_body_str)?;
        Ok(job)
    }

    pub fn run(&self, job: Box<dyn ScheduledJob>) -> () {
        std::thread::spawn(move || {
            let runtime = tokio::runtime::Builder::new_multi_thread()
                .enable_all()
                .build()
                .unwrap();

            runtime.block_on(async {
                log::info!("Running job.execute()");

                let dto =  JobDto {
                    id: -1,
                    guid: Uuid::new_v4(),
                    created_date: Utc::now()
                };
                
                let job_result= job.execute().await;

                let Ok(_) = create_job(dto).await else {
                    log::error!("Saving job result");
                    return;
                };

                if let Err(e) =  {
                    log::error!("Job failed: {}", e);
                } else {
                    log::info!("Job completed successfully");
                }
            });
        });
        ()
    }
}
