#[async_trait::async_trait]
pub trait ScheduledJob: Send + Sync {
    fn validate(&self) -> Result<(), String>;
    async fn parse(&self) -> Result<(), String>;
    async fn execute(&self) -> Result<(), String>;
}

pub trait Logger<T> {
    fn log(&self, item: T);
}