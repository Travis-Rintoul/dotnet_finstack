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

pub struct MockRepositoryFactory {
    pub mock_jobs: MockJobsRepository,
}

impl MockRepositoryFactory {
    pub fn new() -> Self {
        Self {
            mock_jobs: MockJobsRepository::new()
        }
    }
}

impl RepositoryFactoryTrait for MockRepositoryFactory {
    fn create_jobs_repository(&self) -> Box<dyn JobsRepositoryTrait> {
        Box::new(self.mock_jobs.clone())
    }
}