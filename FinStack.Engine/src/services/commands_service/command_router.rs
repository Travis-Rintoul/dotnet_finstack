use crate::{
    commands::{
        CreateJobCommand, CreateJobCommandHandler, ImportFileCommand, ImportFileCommandHandler,
    },
    db::{DbContext, RepositoryFactoryTrait},
    services::commands_service::{traits::CommandHandlerDyn, types::CommandError, CommandHandler},
};
use async_trait::async_trait;
use std::{
    any::{Any, TypeId},
    collections::HashMap,
    sync::Arc,
};

#[allow(dead_code)]
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

pub struct CommandHandlerAdapter<H, C> {
    inner: H,
    _phantom: std::marker::PhantomData<C>,
}

impl<H, C> CommandHandlerAdapter<H, C> {
    pub fn new(inner: H) -> Self {
        Self {
            inner,
            _phantom: Default::default(),
        }
    }
}

#[async_trait]
impl<H, C> CommandHandlerDyn for CommandHandlerAdapter<H, C>
where
    H: CommandHandler<C> + Send + Sync,
    C: Send + Sync + 'static,
{
    async fn handle(
        &self,
        dependencies: Arc<CommandDependencies>,
        cmd: Box<dyn Any + Send>,
    ) -> Result<Box<dyn Any + Send>, CommandError> {
        let cmd = *cmd.downcast::<C>().expect("Command type mismatch");
        self.inner.handle(dependencies, cmd).await?;
        Ok(Box::new(()) as Box<dyn Any + Send>)
    }
}

pub struct CommandRouter {
    pub dependencies: Arc<CommandDependencies>,
    handlers: HashMap<TypeId, Arc<dyn CommandHandlerDyn>>,
}

impl CommandRouter {
    pub fn new(deps: Arc<CommandDependencies>) -> Self {
        let mut handlers: HashMap<TypeId, Arc<dyn CommandHandlerDyn>> = HashMap::new();

        handlers.insert(
            TypeId::of::<ImportFileCommand>(),
            Arc::new(CommandHandlerAdapter::new(ImportFileCommandHandler {})),
        );
        handlers.insert(
            TypeId::of::<CreateJobCommand>(),
            Arc::new(CommandHandlerAdapter::new(CreateJobCommandHandler {})),
        );

        Self {
            handlers,
            dependencies: deps,
        }
    }

    pub async fn send(&self, cmd: Box<dyn Any + Send + Sync>) -> Result<(), CommandError> {
        let type_id = (*cmd).type_id();
        let handler = self
            .handlers
            .get(&type_id)
            .expect("Handler not found")
            .clone();

        handler.handle(Arc::clone(&self.dependencies), cmd).await?;
        Ok(())
    }
}
