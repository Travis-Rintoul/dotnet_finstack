use serde::Deserialize;

use crate::{db::{DbContext, JobsRepository}, models::JobCode};

pub struct JobContext {
    db_context: DbContext,
    jobs: Option<JobsRepository>,
}

#[async_trait::async_trait]
pub trait ScheduledJob: Send + Sync { }

