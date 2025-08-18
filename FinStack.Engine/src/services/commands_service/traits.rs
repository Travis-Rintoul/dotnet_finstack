use crate::services::commands_service::{CommandDependencies, CommandError};
use async_trait::async_trait;
use std::{any::Any, sync::Arc};

pub trait Command: Send + Sync {}

#[async_trait]
pub trait CommandHandler<C>: Send + Sync {
    async fn handle(
        &self,
        dependencies: Arc<CommandDependencies>,
        command: C,
    ) -> Result<Box<dyn Any + Send>, CommandError>;
}

#[async_trait]
pub trait CommandHandlerDyn: Send + Sync {
    async fn handle(
        &self,
        dependencies: Arc<CommandDependencies>,
        cmd: Box<dyn Any + Send>,
    ) -> Result<Box<dyn Any + Send>, CommandError>;
}
