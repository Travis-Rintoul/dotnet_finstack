use std::sync::Arc;

use async_trait::async_trait;
use uuid::Uuid;

use crate::{
    db::{DbContext, JobsRepositoryTrait, RepositoryError},
    models::JobDto,
};

#[allow(dead_code)]
pub struct JobsRepository {
    pub ctx: Arc<DbContext>,
}

impl JobsRepository {
    pub fn new(ctx: Arc<DbContext>) -> Self {
        Self { ctx }
    }
}

#[allow(dead_code)]
#[async_trait]
impl JobsRepositoryTrait for JobsRepository {
    async fn create(&self, entity: JobDto) -> Result<Uuid, RepositoryError> {
        sqlx::query(
            r#"
            INSERT INTO "Jobs" 
                ("Guid", "JobType", "ElapsedMs", "Success", "StartTime", "FinishTime", "Message")
                VALUES 
                ($1, $2, $3, $4, $5, $6, $7)
        "#,
        )
        .bind(entity.guid)
        .bind(entity.job_code)
        .bind(entity.elapsed)
        .bind(entity.success)
        .bind(entity.start_time)
        .bind(entity.finish_time)
        .bind(&entity.message)
        .execute(&self.ctx.pool)
        .await?;

        Ok(entity.guid)
    }

    async fn find_by_id(&self, _id: u32) -> Option<JobDto> {
        None
    }

    async fn find_all(&self) -> Vec<JobDto> {
        vec![]
    }

    async fn update(&self, _entity: JobDto) -> Result<Uuid, RepositoryError> {
        Ok(Uuid::new_v4())
    }

    async fn remove(&self, _entity: JobDto) -> Result<(), RepositoryError> {
        Ok(())
    }
}
