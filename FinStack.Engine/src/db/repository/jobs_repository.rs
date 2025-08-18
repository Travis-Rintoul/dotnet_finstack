use std::sync::Arc;

use async_trait::async_trait;
use uuid::Uuid;

use crate::{
    db::{repository::mock::MockMethod, DbContext, JobsRepositoryTrait, RepositoryError},
    models::JobDto,
};

pub struct JobsRepository {
    pub ctx: Arc<DbContext>
}

impl JobsRepository{
    pub fn new(ctx: Arc<DbContext>) -> Self {
        Self { ctx }
    }
}

#[async_trait]
impl JobsRepositoryTrait for JobsRepository{
    async fn create(&self, entity: JobDto) -> Result<Uuid, RepositoryError> {
        Ok(Uuid::new_v4())
    }

    async fn find_by_id(&self, id: u32) -> Option<JobDto> {
        None
    }

    async fn find_all(&self) -> Vec<JobDto> {
        vec![]
    }

    async fn update(&self, entity: JobDto) -> Result<Uuid, RepositoryError> {
        Ok(Uuid::new_v4())
    }

    async fn remove(&self, entity: JobDto) -> Result<(), RepositoryError> {
        Ok(())
    }
}

#[derive(Clone)]
pub struct MockJobsRepository {
    pub create_method: MockMethod<JobDto, Result<Uuid, RepositoryError>>,
    pub update_method: MockMethod<JobDto, Result<Uuid, RepositoryError>>,
    pub remove_method: MockMethod<JobDto, Result<(), RepositoryError>>,
    pub find_by_id_method: MockMethod<u32, Option<JobDto>>,
    pub find_all_method: MockMethod<(), Vec<JobDto>>,
}

impl MockJobsRepository {
    pub fn new() -> Self {
        Self::default()
    }
}

impl Default for MockJobsRepository {
    fn default() -> Self {
        Self {
            create_method: MockMethod::new(|_| todo!()),
            update_method: MockMethod::new(|_| todo!()),
            remove_method: MockMethod::new(|_| todo!()),
            find_by_id_method: MockMethod::new(|_| None),
            find_all_method: MockMethod::new(|_| vec![]),
        }
    }
}

#[async_trait]
impl JobsRepositoryTrait for MockJobsRepository {
    async fn create(&self, entity: JobDto) -> Result<Uuid, RepositoryError> {
        self.create_method.call(entity)
    }

    async fn update(&self, entity: JobDto) -> Result<Uuid, RepositoryError> {
        self.update_method.call(entity)
    }

    async fn remove(&self, entity: JobDto) -> Result<(), RepositoryError> {
        self.remove_method.call(entity)
    }

    async fn find_by_id(&self, id: u32) -> Option<JobDto> {
        self.find_by_id_method.call(id)
    }

    async fn find_all(&self) -> Vec<JobDto> {
        self.find_all_method.call(())
    }
}