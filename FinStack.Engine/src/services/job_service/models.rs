use std::fmt;
use std::error::Error;
use std::str::FromStr;


#[derive(Debug)]
pub enum JobCode {
    ImportFile
}

impl FromStr for JobCode {
    type Err = String;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        match s {
            "import-file" => Ok(JobCode::ImportFile),
            _ => Err(format!("Unknown job code: {}", s)),
        }
    }
}


#[derive(Debug, Clone, Copy)]
pub struct JobCodeParseError;

impl fmt::Display for JobCodeParseError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "Failed to parse JobCode")
    }
}

impl Error for JobCodeParseError {}

#[derive(Debug, Clone, Copy)]
pub struct JobBodyParseError;

impl fmt::Display for JobBodyParseError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "Failed to parse JobBod")
    }
}

impl Error for JobBodyParseError {}