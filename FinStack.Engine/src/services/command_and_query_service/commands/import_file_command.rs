use std::{error::Error, sync::Arc};

use async_trait::async_trait;
use serde::Deserialize;

use crate::services::command_and_query_service::{traits::CommandTrait, CQRSDependencies};

#[derive(Deserialize, Debug)]
pub struct ImportFileCommand {
    pub file_name: String,
}

#[async_trait]
impl CommandTrait for ImportFileCommand {
    async fn handle(
        &self,
        _: Arc<CQRSDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        if self.file_name == "" {
            return Err("file_name required".into());
        }

        Ok(())
    }
}

#[cfg(test)]
mod tests {

    use crate::{db::MockUserRepository, services::command_and_query_service::CQRSDispatcher};

    use super::*;
    use std::sync::Arc;

    async fn setup() -> CQRSDispatcher {
        let dependencies = Arc::new(CQRSDependencies::mock());
        CQRSDispatcher::new(dependencies)
    }

    #[tokio::test]
    async fn should_pass() {
        let dispatcher = setup().await;
        let command = ImportFileCommand {
            file_name: "test_file.txt".to_string(),
        };

        let result = dispatcher.send_command(command).await;
        assert!(result.is_ok(), "Handler should return Ok result");
    }

    #[tokio::test]
    async fn should_fail_when_no_file_provided() {
        let dispatcher = setup().await;
        let command = ImportFileCommand {
            file_name: "".to_string(),
        };

        let result = dispatcher.send_command(command).await;
        assert!(result.is_err(), "Command should return Err result");
    }
}
