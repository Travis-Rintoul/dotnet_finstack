use std::{fmt::{Display, Formatter, Result as FmtResult}, str::FromStr};

pub enum JobCode {
    ImportFile
}

impl FromStr for JobCode {
    type Err = String;
    fn from_str(s: &str) -> Result<Self, Self::Err> {
        match s {
            "import-file" => Ok(JobCode::ImportFile),
            _ => Err("unable to match JobCode".to_owned())
        }
    }
}

impl Display for JobCode {
    fn fmt(&self, f: &mut Formatter<'_>) -> FmtResult {
        match self {
            JobCode::ImportFile => write!(f, "import-file"),
        }
    }
}