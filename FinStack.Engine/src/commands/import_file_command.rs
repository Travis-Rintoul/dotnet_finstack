use async_trait::async_trait;
use serde::Deserialize;

use crate::services::command_router::{CommandArgs, CommandDependencies, CommandError, CommandHandler};

#[derive(Deserialize, Debug)]
pub struct ImportFileCommand{
    pub file_name: String,
}

impl CommandArgs for ImportFileCommand {}

pub struct ImportFileCommandHandler;

#[async_trait]
impl<'a> CommandHandler<'a, ImportFileCommand, ()> for ImportFileCommandHandler {
    async fn execute(dependencies: &'a CommandDependencies, arguments: ImportFileCommand) -> Result<(), CommandError> {
        log::error!("Executing ImportFileCommand: {:#?}", arguments);

 

        Ok(())
    }
}
