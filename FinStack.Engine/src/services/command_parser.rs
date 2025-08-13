use std::{any::Any, collections::HashMap, error::Error};
use serde::Deserialize;

use crate::{commands::ImportFileCommand, services::command_router::{CommandError}};

type ParserFn = fn(&str) -> Result<Box<dyn Any + Send + Sync>, CommandError>;

#[derive(Deserialize)]
#[serde(deny_unknown_fields)]
struct CommandInput {
    command_name: String,
}

pub struct CommandParser {
    parsers: HashMap<&'static str, ParserFn>,
}

impl CommandParser {
    pub fn new() -> Self {
        let mut parsers: HashMap<&'static str, ParserFn> = HashMap::new();

        parsers.insert("import-file", |json| {
            let cmd: ImportFileCommand = serde_json::from_str(json)
                .map_err(|e| Box::new(e) as CommandError)?;
            let cmd_box: Box<dyn Any + Send + Sync> = Box::new(cmd);
            Ok(cmd_box)
        });

        Self { parsers }
    }

    pub fn parse(&self, command_name: &str, json: &str) -> Result<Box<dyn Any + Send + Sync>, CommandError> {
        
        let parser = self.parsers.get(&command_name)
            .ok_or_else(|| {
                Box::new(std::io::Error::new(
                    std::io::ErrorKind::InvalidInput,
                    format!("Unknown command: {}", command_name),
                )) as CommandError
            })?;

        let cmd = parser(json)?;
        Ok(cmd)
    }
}