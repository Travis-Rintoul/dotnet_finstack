use serde::Deserialize;
use std::{
    collections::HashMap,
    io::{Error, ErrorKind},
};

use crate::{
    commands::ImportFileCommand,
    services::commands_service::{types::CommandError, Command, ParserFn},
};

#[allow(dead_code)]
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
            let cmd: ImportFileCommand =
                serde_json::from_str(json).map_err(|e| Box::new(e) as CommandError)?;
            let cmd_box: Box<dyn Command> = Box::new(cmd);
            Ok(cmd_box)
        });

        Self { parsers }
    }

    pub fn parse(&self, command_name: &str, json: &str) -> Result<Box<dyn Command>, CommandError> {
        let parser = self.parsers.get(&command_name).ok_or_else(|| {
            Box::new(Error::new(
                ErrorKind::InvalidInput,
                format!("Unknown command: {}", command_name),
            )) as CommandError
        })?;

        let cmd = parser(json)?;
        Ok(cmd)
    }
}
