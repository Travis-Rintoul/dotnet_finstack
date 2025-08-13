use std::{any::Any, sync::Arc};

use async_trait::async_trait;
use serde::Deserialize;

use crate::services::command_router::{CommandDependencies, CommandError, CommandHandler};

#[derive(Deserialize, Debug)]
pub struct ImportFileCommand{
    pub file_name: String,
}

pub struct ImportFileCommandHandler;

#[async_trait]
impl<'a> CommandHandler<ImportFileCommand> for ImportFileCommandHandler {
    async fn handle(&self, _: Arc<CommandDependencies>, command: ImportFileCommand) -> Result<Box<dyn Any + Send>, CommandError> {
        log::error!("QQQQQQQQQQQ");
        Ok(Box::new(()) as Box<dyn Any + Send>)
    }
}