use std::sync::Arc;

use async_trait::async_trait;
use uuid::Uuid;

use crate::{
    db::{repository::mock::MockMethod, DbContext, JobsRepositoryTrait},
    models::JobDto,
};

pub struct JobsRepository {
    pub ctx: Arc<DbContext>
}


impl JobsRepository {
    pub fn new(ctx: Arc<DbContext>) -> Self {
        Self { ctx }
    }
}

#[async_trait]
impl<'a> JobsRepositoryTrait for JobsRepository {
    async fn create(&self, entity: &JobDto) -> Result<Uuid, sqlx::Error> {
        Ok(Uuid::new_v4())
    }

    async fn find_by_id(&self, id: u32) -> Option<JobDto> {
        None
    }

    async fn find_all(&self) -> Vec<JobDto> {
        vec![]
    }

    async fn update(&self, entity: &JobDto) -> Result<Uuid, sqlx::Error> {
        Ok(Uuid::new_v4())
    }

    async fn remove(&self, entity: &JobDto) -> Result<(), sqlx::Error> {
        Ok(())
    }
}

pub struct MockJobsRepository {
    pub create_method: MockMethod<JobDto, Result<Uuid, String>>,
    pub find_by_id_method: MockMethod<JobDto, Result<Uuid, String>>,
    pub find_all_method: MockMethod<(), Result<Uuid, String>>,
    pub update_method: MockMethod<JobDto, Result<Uuid, String>>,
    pub remove_method: MockMethod<JobDto, Result<Uuid, String>>,
}


impl MockJobsRepository {
    pub fn new() -> Self {
        Self::default()
    }
}

impl Default for MockJobsRepository {
    fn default() -> Self {
        Self {
            create_method: MockMethod::new(|_| todo!("create_method not initialized")),
            find_by_id_method: MockMethod::new(|_| todo!("find_by_id_method not initialized")),
            find_all_method: MockMethod::new(|_| todo!("find_all_method not initialized")),
            update_method: MockMethod::new(|_| todo!("update_method not initialized")),
            remove_method: MockMethod::new(|_| todo!("remove_method not initialized")),
        }
    }
}

#[async_trait]
impl JobsRepositoryTrait for MockJobsRepository {
    async fn create(&self, entity: &JobDto) -> Result<Uuid, sqlx::Error> {
        Ok(Uuid::new_v4())
    }

    async fn find_by_id(&self, id: u32) -> Option<JobDto> {
        None
    }

    async fn find_all(&self) -> Vec<JobDto> {
        vec![]
    }

    async fn update(&self, entity: &JobDto) -> Result<Uuid, sqlx::Error> {
        Ok(Uuid::new_v4())
    }

    async fn remove(&self, entity: &JobDto) -> Result<(), sqlx::Error> {
        Ok(())
    }
}