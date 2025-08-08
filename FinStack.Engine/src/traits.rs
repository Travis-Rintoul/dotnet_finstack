use serde::Deserialize;

use crate::models::JobCode;

#[async_trait::async_trait]
pub trait ScheduledJob: Send + Sync + for<'a> Deserialize<'a> {
    fn validate(&self) -> Result<(), String>;
    async fn execute(&self) -> Result<(), String>;
    fn code(&self) -> JobCode;
}

pub trait Logger<T> {
    fn log(&self, item: T);
}