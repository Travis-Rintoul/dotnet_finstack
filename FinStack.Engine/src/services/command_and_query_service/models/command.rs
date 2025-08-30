use std::{
    error::Error,
    fmt::{self, Display, Formatter},
    sync::Arc,
};

use async_trait::async_trait;
use serde::Deserialize;

use crate::services::command_and_query_service::{
    CQRSDependencies,
    commands::{CreateJobCommand, ImportFileCommand, TestCommand},
    traits::CommandTrait,
};

#[derive(Debug, Deserialize)]
#[serde(tag = "command_name")]
pub enum Command {
    #[serde(rename = "create-job")]
    CreateJob(CreateJobCommand),
    #[serde(rename = "import-file")]
    ImportFile(ImportFileCommand),
    #[serde(rename = "test")]
    Test(TestCommand),
}

impl Display for Command {
    fn fmt(&self, f: &mut Formatter<'_>) -> fmt::Result {
        match self {
            Command::CreateJob(cmd) => write!(f, "create-job: {:?}", cmd),
            Command::ImportFile(cmd) => write!(f, "import-file: {:?}", cmd),
            Command::Test(cmd) => write!(f, "test: {:?}", cmd)
        }
    }
}

#[async_trait]
impl CommandTrait for Command {
    async fn handle(
        &self,
        services: Arc<CQRSDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        match self {
            Command::CreateJob(cmd) => cmd.handle(services).await,
            Command::ImportFile(cmd) => cmd.handle(services).await,
            Command::Test(cmd) => cmd.handle(services).await
        }
    }
}
