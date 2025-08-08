use std::{time::Duration};

use async_trait::async_trait;
use serde::Deserialize;
use tokio::time::sleep;

use crate::{models::JobCode, traits::ScheduledJob};

#[derive(Deserialize)]
pub struct ImportFileJob {
    file_name: String,
}

#[async_trait]
impl ScheduledJob for ImportFileJob {

    fn code(&self) -> JobCode {
        JobCode::ImportFile
    }

    fn validate(&self) -> Result<(), String> {
        if self.file_name.is_empty() {
            return Err("File name required".to_string());
        }

        Ok(())
    }

    async fn execute(&self) -> Result<(), String> {
        log::info!("Executing ImportFileJob!");

        sleep(Duration::from_secs(10)).await;

        Ok(())
    }
}
