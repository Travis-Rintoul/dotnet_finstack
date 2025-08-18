use std::error::Error;

use crate::services::commands_service::{Command};

pub type ParserFn = fn(&str) -> Result<Box<dyn Command>, CommandError>;
pub type CommandError = Box<dyn Error + Send + Sync>;
