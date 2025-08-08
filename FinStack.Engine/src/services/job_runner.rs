use crate::traits::ScheduledJob;
use chrono::{DateTime, Utc};

pub struct JobResult {
    pub success: bool,
    pub elapsed: i64,
    pub start_time: DateTime<Utc>,
    pub finish_time: DateTime<Utc>,
    pub message: String
}

pub struct JobRunner;

impl JobRunner {
    pub fn new() -> Self {
        JobRunner
    }

    pub async fn execute(&self, job: Box<dyn ScheduledJob>) -> JobResult {
        let start_time = Utc::now();
        log::info!("Job Started {:?} at {}", job.code(), start_time);

        let job_result = job.execute().await;

        let finish_time = Utc::now();
        let elapsed = finish_time - start_time;

        log::info!("Job Finished {:?} at {} elapsed {:.3}", job.code(), finish_time, elapsed);

        let elapsed_us: i64 = elapsed.num_microseconds().unwrap_or(0);

        match job_result {
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
        }
    }
}
