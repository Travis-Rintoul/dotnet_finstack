use std::fmt;
use std::error::Error;

#[derive(Debug)]
pub struct JobCodeParseError;

impl fmt::Display for JobCodeParseError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "Failed to parse JobCode")
    }
}

impl Error for JobCodeParseError {}

#[derive(Debug)]
pub struct JobJsonError;

impl fmt::Display for JobJsonError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(f, "Failed to parse job JSON")
    }
}

impl Error for JobJsonError {}

#[derive(Debug)]
pub enum ParseError {
    JobCodeError(JobCodeParseError),
    JobJsonError(JobJsonError),
}

impl fmt::Display for ParseError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        match self {
            ParseError::JobCodeError(e) => write!(f, "Job code error: {}", e),
            ParseError::JobJsonError(e) => write!(f, "Job JSON error: {}", e),
        }
    }
}

impl Error for ParseError {}

impl From<JobCodeParseError> for ParseError {
    fn from(e: JobCodeParseError) -> Self {
        ParseError::JobCodeError(e)
    }
}

impl From<JobJsonError> for ParseError {
    fn from(e: JobJsonError) -> Self {
        ParseError::JobJsonError(e)
    }
}