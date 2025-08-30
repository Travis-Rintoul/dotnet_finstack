use std::sync::Arc;

use crate::db::{DbContext, JobsRepository, JobsRepositoryTrait, MockJobsRepository, MockUserRepository, UserRepository, UsersRepositoryTrait};

#[allow(dead_code)]
pub struct CQRSDependencies {
    pub user_repository: Arc<dyn UsersRepositoryTrait>,
    pub jobs_repository: Arc<dyn JobsRepositoryTrait>,
}

impl CQRSDependencies {
    pub fn concrete(db: Arc<DbContext>) -> Self {
        Self {
            user_repository: Arc::new(UserRepository::new(Arc::clone(&db))),
            jobs_repository: Arc::new(JobsRepository::new(Arc::clone(&db)))
        }
    }

    pub fn mock() -> Self {
        Self {
            user_repository: Arc::new(MockUserRepository::new()),
            jobs_repository: Arc::new(MockJobsRepository::new())
        }
    }
}

