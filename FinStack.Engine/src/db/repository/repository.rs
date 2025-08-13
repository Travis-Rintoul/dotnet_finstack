use crate::db::{DbContext, JobsRepositoryTrait};

pub trait Entity {}

// pub trait RepositoryFactoryTrait: Send + Sync {
//     fn jobs(&self, db_context: DbContext) -> Box<dyn JobsRepositoryTrait>;
// }
