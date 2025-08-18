use std::{any::Any, sync::Arc};

use crate::services::commands_service::{Command, CommandDependencies, CommandError, CommandHandler};
use async_trait::async_trait;
use serde::Deserialize;

#[derive(Deserialize, Debug)]
pub struct ImportFileCommand {
    pub file_name: String,
}

impl Command for ImportFileCommand {}

pub struct ImportFileCommandHandler;

#[async_trait]
impl<'a> CommandHandler<ImportFileCommand> for ImportFileCommandHandler {
    async fn handle(
        &self,
        _: Arc<CommandDependencies>,
        command: ImportFileCommand,
    ) -> Result<Box<dyn Any + Send>, CommandError> {
        if command.file_name == "" {
            return Err("file_name required".into());
        }

        Ok(Box::new(()) as Box<dyn Any + Send>)
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

        let result = router.send(Box::new(command)).await;
        assert!(result.is_ok(), "Handler should return Ok result");
    }

    #[tokio::test]
    async fn should_fail_when_no_file_provided() {
        let router = setup().await;
        let command = ImportFileCommand {
            file_name: "".to_string(),
        };

        let result = router.send(Box::new(command)).await;
        assert!(result.is_err(), "Command should return Err result");
    }
}
