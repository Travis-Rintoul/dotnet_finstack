use std::sync::Arc;

use crate::db::{
    repository::user_repository::MockUserRepository, DbContext, JobsRepository, JobsRepositoryTrait, MockJobsRepository, UserRepository, UsersRepositoryTrait
};

pub trait RepositoryFactoryTrait: Send + Sync {
    fn create_jobs_repository(&self) -> Box<dyn JobsRepositoryTrait>;
    fn create_users_repository(&self) -> Box<dyn UsersRepositoryTrait>;
}

pub struct RepositoryFactory {
    ctx: Arc<DbContext>,
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

    fn create_users_repository(&self) -> Box<dyn UsersRepositoryTrait> {
        Box::new(UserRepository::new(Arc::clone(&self.ctx)))
    }
}

pub struct MockRepositoryFactory {
    pub mock_jobs: MockJobsRepository,
    pub mock_users: MockUserRepository
}

impl MockRepositoryFactory {
    pub fn new() -> Self {
        Self {
            mock_jobs: MockJobsRepository::new(),
            mock_users: MockUserRepository::new()
        }
    }
}

impl RepositoryFactoryTrait for MockRepositoryFactory {
    fn create_jobs_repository(&self) -> Box<dyn JobsRepositoryTrait> {
        Box::new(self.mock_jobs.clone())
    }

    fn create_users_repository(&self) -> Box<dyn UsersRepositoryTrait> {
        Box::new(self.mock_users.clone())
    }
}
