use std::{error::Error, sync::Arc};

use async_trait::async_trait;
use serde::Deserialize;
use tokio::time::{sleep, Duration};

use crate::services::command_and_query_service::{traits::CommandTrait, CQRSDependencies};

#[derive(Deserialize, Debug)]
pub struct TestCommand {
    pub sleep_seconds: u64,
}

#[async_trait]
impl CommandTrait for TestCommand {
    async fn handle(
        &self,
        _: Arc<CQRSDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        sleep(Duration::from_secs(self.sleep_seconds)).await;
        Ok(())
    }
}