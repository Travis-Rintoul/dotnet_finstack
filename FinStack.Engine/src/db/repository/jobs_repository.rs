use async_trait::async_trait;
use sqlx::Error;
use uuid::Uuid;

use crate::{
    db::DbContext,
    models::{Job, JobDto},
};

#[async_trait]
pub trait JobsRepositoryTrait: Send + Sync  {
    async fn create(&self, entity: JobDto) -> Result<Uuid, sqlx::Error>;
    async fn find_by_id(&self, id: u32) -> Option<JobDto>;
    async fn find_all(&self) -> Vec<JobDto>;
    async fn update(&self, entity: JobDto) -> Result<Uuid, sqlx::Error>;
    async fn remove(&self, entity: JobDto) -> Result<(), sqlx::Error>;
}

pub struct JobsRepository {
    pub ctx: DbContext,
}

impl JobsRepository {
    pub fn new(ctx: DbContext) -> Self {
        Self { ctx }
    }
}

#[async_trait]
impl JobsRepositoryTrait for JobsRepository {

    async fn create(&self, job: JobDto) -> Result<Uuid, Error> {
        Ok(Uuid::new_v4()) // or dummy data
    }

    async fn find_by_id(&self, id: u32) -> Option<JobDto> {
        None
    }

    async fn find_all(&self) -> Vec<JobDto> {
        vec![]
    }

    async fn update(&self, job: JobDto) -> Result<Uuid, Error> {
        Ok(Uuid::new_v4())
    }

    async fn remove(&self, job: JobDto) -> Result<(), Error> {
        Ok(())
    }
}

pub struct MockJobsRepository {
    pub ctx: DbContext,
}

impl MockJobsRepository {
    pub fn new (ctx: DbContext) -> Self {
        Self { ctx }
    }
}

#[async_trait]
impl JobsRepositoryTrait for MockJobsRepository {
    async fn create(&self, job: JobDto) -> Result<Uuid, Error> {
        Ok(Uuid::new_v4()) // or dummy data
    }

    async fn find_by_id(&self, id: u32) -> Option<JobDto> {
        None
    }

    async fn find_all(&self) -> Vec<JobDto> {
        vec![]
    }

    async fn update(&self, job: JobDto) -> Result<Uuid, Error> {
        Ok(Uuid::new_v4())
    }

    async fn remove(&self, job: JobDto) -> Result<(), Error> {
        Ok(())
    }
}

// impl Repository<JobDto, Job> for JobsRepository {
//     async fn create(&self, job: JobDto) -> Result<Uuid, Error> {
//         let sql = r#"
//             INSERT INTO "Jobs" 
//                 ("Guid", "JobType", "Success", "ElapsedMs", "StartTime", "FinishTime", "Message")
//                 VALUES 
//                 ($1, $2, $3, $4, $5, $6, $7)
//         "#;

//         let query = sqlx::query(sql)
//             .bind(job.guid)
//             .bind(job.job_code)
//             .bind(job.success)
//             .bind(job.elapsed)
//             .bind(job.start_time)
//             .bind(job.finish_time)
//             .bind(job.message);

//         let query_result = query    
//             .execute(&self.ctx.pool)
//             .await;

//         return match query_result {
//             Ok(_) => Ok(job.guid),
//             Err(e) => Err(e)
//         }
//     }

//     async fn find_by_id(&self, id: u32) -> Option<JobDto> {
//         todo!()
//     }

//     async fn find_all(&self) -> Vec<JobDto> {
//         todo!()
//     }

//     async fn update(&self, entity: JobDto) -> Result<Uuid, Error>  {
//         todo!()
//     }

//     async fn remove(&self, entity: JobDto) -> Result<(), Error>  {
//         todo!()
//     }
// }
