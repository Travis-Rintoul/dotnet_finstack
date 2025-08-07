use async_trait::async_trait;
use serde::Deserialize;

use log::{info};
use crate::traits::ScheduledJob;

#[derive(Deserialize)]
pub struct ImportFileJob {
    file_name: String,
}

#[async_trait]
impl ScheduledJob for ImportFileJob {
    fn validate(&self) -> Result<(), String> {
        Ok(())
    }

    async fn parse(&self) -> Result<(), String> {
        Ok(())
    }

    async fn execute(&self) -> Result<(), String> {
        log::info!("Executing ImportFileJob!");
        Ok(())
    }
}
