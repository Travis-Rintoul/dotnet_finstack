

use std::error::Error;
use std::str::FromStr;

use async_trait::async_trait;
use serde::de::DeserializeOwned;
use serde::Deserialize;

use crate::commands::ImportFileCommand;
use crate::commands::ImportFileCommandHandler;
use crate::db::DbContext;
use crate::db::RepositoryFactory;
use crate::db::RepositoryFactoryTrait;

#[derive(Deserialize)]
struct CommandInput {
    command_name: String,
    params: serde_json::Value, // raw JSON to deserialize later per command
}

pub enum Command {
    ImportFile(ImportFileCommand),
}

impl FromStr for Command {
    type Err = CommandError;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        // Parse the wrapper JSON to get command_name and raw params
        let input: CommandInput = serde_json::from_str(s).map_err(|e| Box::new(e) as CommandError)?;

        // A helper to deserialize params into a concrete type and wrap in enum
        fn deserialize_command<T>(
            params: &serde_json::Value,
            wrap: impl FnOnce(T) -> Command,
        ) -> Result<Command, CommandError>
        where
            T: DeserializeOwned,
        {
            let args = serde_json::from_value::<T>(params.clone())
                .map_err(|e| Box::new(e) as CommandError)?;
            Ok(wrap(args))
        }

        match input.command_name.as_str() {
            "import-file" => deserialize_command::<ImportFileCommand>(&input.params, Command::ImportFile),
            // Add other commands here, e.g.
            // "other-command" => deserialize_command::<OtherCommand>(&input.params, Command::OtherVariant),
            _ => Err(Box::new(std::io::Error::new(std::io::ErrorKind::InvalidInput, "Unknown command"))),
        }
    }
}



pub trait CommandParser {
    fn parse(json: &str) -> Result<Command, Box<dyn Error>>;
}

pub trait CommandArgs {}

#[derive(Default)]
pub struct CommandDependencies {

    pub db: Option<DbContext>,
    pub repository_factory: Option<Box<dyn RepositoryFactoryTrait>>
}

impl CommandDependencies {
    pub fn new(db: DbContext, repository_factory: Box<dyn RepositoryFactoryTrait>) -> Self {
        Self {
            db: Some(db),
            repository_factory: Some(repository_factory),
        }
    }
}

pub type CommandError = Box<dyn Error + Send + Sync>;

pub struct CommandRouter {
    pub dependencies: CommandDependencies,
}

impl CommandRouter {
    pub fn new(deps: CommandDependencies) -> Self {
        Self {
            dependencies: deps,
        }
    }

    pub async fn send(&self, command: Command) -> Result<(), CommandError> {
        match command {
            Command::ImportFile(cmd) => {
                ImportFileCommandHandler::execute(&self.dependencies, cmd).await
            }
        }
    }

        // fn get_job_from_json(code: JobCode, json: &str) -> Result<Box<dyn ScheduledJob>, String> {
    //     match code {
    //         // JobCode::ImportFile => Self::deserialize_job::<ImportFileJob>(json),
    //         _ => Err("E".to_string())
    //     }
    // }




}
#[async_trait]
pub trait CommandHandler<'a, C, R> {
    async fn execute(dependencies: &'a CommandDependencies, arguments: C) -> Result<R, CommandError>;
}
