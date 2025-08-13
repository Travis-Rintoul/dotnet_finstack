use std::sync::Arc;

use crate::db::{DbContext, JobsRepository, JobsRepositoryTrait, MockJobsRepository};

pub trait RepositoryFactoryTrait: Send + Sync {
    fn create_jobs_repository(&self) -> Box<dyn JobsRepositoryTrait>;
}

pub struct RepositoryFactory {
    ctx: Arc<DbContext>
}

impl RepositoryFactory {
    pub fn new(db_context: Arc<DbContext>) -> Self {
        Self { ctx: db_context }
    }
}

impl RepositoryFactoryTrait for RepositoryFactory {
    fn create_jobs_repository(&self) -> Box<dyn JobsRepositoryTrait> {
        Box::new(JobsRepository::new(Arc::clone(&self.ctx)))
    }
}

pub struct MockRepositoryFactory;

impl MockRepositoryFactory {
    pub fn new() -> Self {
        Self
    }
}

impl RepositoryFactoryTrait for MockRepositoryFactory {
    fn create_jobs_repository(&self) -> Box<dyn JobsRepositoryTrait> {
        Box::new(MockJobsRepository::new())
    }
}
