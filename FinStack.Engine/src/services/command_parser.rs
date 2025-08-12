use std::{io};

use serde::de::DeserializeOwned;

use crate::{commands::ImportFileCommand, services::command_router::{Command, CommandArgs, CommandError}};


pub struct CommandParser;

impl CommandParser {
    pub fn parse(command_name: &str, json: &str) -> Result<Command, CommandError> {
        fn deserialize_command<T: CommandArgs + DeserializeOwned + 'static>(json: &str) -> Result<T, CommandError> {
            serde_json::from_str::<T>(json)
                .map(|t| t)
                .map_err(|e| Box::new(e) as CommandError)
        }

        match command_name {
            "import-file" => {
                let args = deserialize_command::<ImportFileCommand>(json)?;
                Ok(Command::ImportFile(args))
            }
            _ => Err(Box::new(io::Error::new(io::ErrorKind::InvalidInput, "Unknown command"))),
        }
    }
}