use crate::{
    commands::{CreateJobCommand, ImportFileCommand},
    db::{DbContext, RepositoryFactoryTrait},
};
use async_trait::async_trait;
use serde::Deserialize;
use std::{
    error::Error,
    fmt::{self, Display, Formatter},
    sync::Arc,
};

#[derive(Default)]
pub struct CommandDependencies {
    pub db: Option<Arc<DbContext>>,
    pub repository_factory: Option<Box<dyn RepositoryFactoryTrait>>,
}

impl CommandDependencies {
    pub fn new(db: Arc<DbContext>, repository_factory: Box<dyn RepositoryFactoryTrait>) -> Self {
        Self {
            db: Some(db),
            repository_factory: Some(repository_factory),
        }
    }
}

#[derive(Debug, Deserialize)]
#[serde(tag = "command_name")]
pub enum Command {
    #[serde(rename = "create-job")]
    CreateJob(CreateJobCommand),
    #[serde(rename = "import-file")]
    ImportFile(ImportFileCommand),
}

impl Display for Command {
    fn fmt(&self, f: &mut Formatter<'_>) -> fmt::Result {
        match self {
            Command::CreateJob(cmd) => write!(f, "create-job: {:?}", cmd),
            Command::ImportFile(cmd) => write!(f, "import-file: {:?}", cmd),
        }
    }
}

#[async_trait]
impl CommandTrait for Command {
    async fn handle(
        &self,
        services: Arc<CommandDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        match self {
            Command::CreateJob(cmd) => cmd.handle(services).await,
            Command::ImportFile(cmd) => cmd.handle(services).await,
        }
    }
}

#[async_trait]
pub trait CommandTrait: Send + Sync {
    async fn handle(
        &self,
        services: Arc<CommandDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>>;
}

pub struct CommandRouter {
    services: Arc<CommandDependencies>,
}

impl CommandRouter {
    pub fn new(services: Arc<CommandDependencies>) -> Self {
        Self { services }
    }

    pub async fn send<C>(&self, command: C) -> Result<(), Box<dyn Error + Send + Sync>>
    where
        C: CommandTrait,
    {
        command.handle(Arc::clone(&self.services)).await
    }
}
