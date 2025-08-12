use async_trait::async_trait;
use uuid::Uuid;

use crate::db::{DbContext, JobsRepository, JobsRepositoryTrait, MockJobsRepository};

pub trait Entity {}


pub trait RepositoryFactoryTrait: Send + Sync {
    fn jobs(&self, db_context: DbContext) -> Box<dyn JobsRepositoryTrait>;
}

pub struct RepositoryFactory;

impl RepositoryFactory {
    pub fn new() -> Self {
        Self
    }
}

impl RepositoryFactoryTrait for RepositoryFactory {
    fn jobs(&self, db_context: DbContext) -> Box<dyn JobsRepositoryTrait> {
        Box::new(JobsRepository::new(db_context))
    }
}

pub struct MockRepositoryFactory;

impl MockRepositoryFactory {
    pub fn new() -> Self {
        Self
    }
}

#[async_trait]
impl RepositoryFactoryTrait for MockRepositoryFactory {
    fn jobs(&self, db_context: DbContext) -> Box<dyn JobsRepositoryTrait> {
        Box::new(MockJobsRepository::new(db_context))
    }
}