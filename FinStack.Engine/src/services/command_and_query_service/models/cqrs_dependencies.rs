use std::sync::Arc;

use crate::db::{DbContext, RepositoryFactoryTrait};

#[allow(dead_code)]
#[derive(Default)]
pub struct CQRSDependencies {
    pub db: Option<Arc<DbContext>>,
    pub repository_factory: Option<Box<dyn RepositoryFactoryTrait>>,
}

impl CQRSDependencies {
    pub fn new(db: Arc<DbContext>, repository_factory: Box<dyn RepositoryFactoryTrait>) -> Self {
        Self {
            db: Some(db),
            repository_factory: Some(repository_factory),
        }
    }
}

