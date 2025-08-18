use std::{error::Error, sync::Arc};

use crate::services::commands_service::{CommandDependencies, CommandTrait};
use async_trait::async_trait;
use serde::Deserialize;

#[derive(Deserialize, Debug)]
pub struct ImportFileCommand {
    pub file_name: String,
}

#[async_trait]
impl CommandTrait for ImportFileCommand {
    async fn handle(
        &self,
        _: Arc<CommandDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        if self.file_name == "" {
            return Err("file_name required".into());
        }

        Ok(())
    }
}

#[cfg(test)]
mod tests {

    use crate::services::commands_service::CommandRouter;

    use super::*;
    use std::sync::Arc;

    async fn setup() -> CommandRouter {
        let dependencies = Arc::new(CommandDependencies::default());
        CommandRouter::new(dependencies)
    }

    #[tokio::test]
    async fn should_pass() {
        let router = setup().await;
        let command = ImportFileCommand {
            file_name: "test_file.txt".to_string(),
        };

        let result = router.send(command).await;
        assert!(result.is_ok(), "Handler should return Ok result");
    }

    #[tokio::test]
    async fn should_fail_when_no_file_provided() {
        let router = setup().await;
        let command = ImportFileCommand {
            file_name: "".to_string(),
        };

        let result = router.send(command).await;
        assert!(result.is_err(), "Command should return Err result");
    }
}
