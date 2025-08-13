// use std::collections::HashMap;
// use std::error::Error;
// use std::str::FromStr;
// use std::sync::Arc;

// use async_trait::async_trait;
// use serde::Deserialize;
// use serde::de::DeserializeOwned;
// use std::any::{Any, TypeId};
// use crate::commands::CreateJobCommand;
// use crate::commands::CreateJobCommandHandler;
// use crate::commands::ImportFileCommand;
// use crate::commands::ImportFileCommandHandler;
// use crate::db::DbContext;
// use crate::db::RepositoryFactoryTrait;

// #[derive(Deserialize)]
// struct CommandInput {
//     command_name: String,
//     arguments: serde_json::Value, // raw JSON to deserialize later per command
// }

// #[derive(Debug)]
// pub enum Command {
//     ImportFile(ImportFileCommand),
//     CreateJob(CreateJobCommand),
// }

// impl Command {
//     pub fn name(&self) -> String {
//         match self {
//             Command::ImportFile(_) => "import-file".to_string(),
//             Command::CreateJob(_) => "create-job".to_string()
//         }
//     }
// }


// impl FromStr for Command {
//     type Err = CommandError;

//     fn from_str(s: &str) -> Result<Self, Self::Err> {
//         let input: CommandInput =
//             serde_json::from_str(s).map_err(|e| Box::new(e) as CommandError)?;

//         fn deserialize<T>(
//             arguments: &serde_json::Value,
//             wrap: impl FnOnce(T) -> Command,
//         ) -> Result<Command, CommandError>
//         where
//             T: DeserializeOwned,
//         {
//             let args = serde_json::from_value::<T>(arguments.clone())
//                 .map_err(|e| Box::new(e) as CommandError)?;
//             Ok(wrap(args))
//         }

//         match input.command_name.as_str() {
//             "import-file" => deserialize::<ImportFileCommand>(&input.arguments, Command::ImportFile),
//             _ => Err(Box::new(std::io::Error::new(
//                 std::io::ErrorKind::InvalidInput,
//                 "Unknown command",
//             ))),
//         }
//     }
// }

// #[async_trait]
// pub trait CommandHandlerDyn: Send + Sync {
//     async fn handle(&self, cmd: Box<dyn Any + Send>) -> Result<Box<dyn Any + Send>, CommandError>;
// }

// pub struct CommandHandlerAdapter<H, C> {
//     inner: H,
//     _phantom: std::marker::PhantomData<C>,
// }

// impl<H, C> CommandHandlerAdapter<H, C> {
//     pub fn new(inner: H) -> Self {
//         Self { inner, _phantom: Default::default() }
//     }
// }

// #[async_trait]
// impl<H, C> CommandHandlerDyn for CommandHandlerAdapter<H, C>
// where
//     H: CommandHandler<C> + Send + Sync,
//     C: Send + 'static,
// {
//     async fn handle(&self, cmd: Box<dyn Any + Send>) -> Result<Box<dyn Any + Send>, CommandError> {
//         let cmd = *cmd.downcast::<C>().expect("Command type mismatch");
//         self.inner.execute(cmd).await?;
//         Ok(Box::new(()) as Box<dyn Any + Send>)
//     }
// }

// pub trait CommandParser {
//     fn parse(json: &str) -> Result<Command, Box<dyn Error>>;
// }

// pub trait CommandArgs: Any + Send + Sync {
//     // optional: helper to get TypeId
//     fn type_id(&self) -> TypeId {
//         TypeId::of::<Self>()
//     }
// }

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

// pub type CommandError = Box<dyn Error + Send + Sync>;

// pub struct CommandRouter {
//     pub dependencies: CommandDependencies,
//     handlers: HashMap<TypeId, Box<dyn CommandHandler>>,
// }

// impl CommandRouter {
//     pub fn new(deps: CommandDependencies) -> Self {
//         Self { dependencies: deps }
//     }

//     pub async fn send<T>(&self, command: impl CommandArgs) -> Result<T, CommandError>
//     where
//         T: Default,
//     {
//         let opt = self.handlers.get(command.type_id()).ok_or(Err("Q").into());

//         match command {
//             Command::ImportFile(cmd) => {
//                 ImportFileCommandHandler::handle(&self.dependencies, cmd).await?;
//             }
//             Command::CreateJob(cmd) => {
//                 CreateJobCommandHandler::handle(&self.dependencies, cmd).await?;
//             }
//         }
//         Ok(T::default())
//     }
// }

// #[async_trait]
// pub trait CommandHandler: Send + Sync {
//     async fn handle(&self, cmd: Box<dyn Any + Send>) -> Result<Box<dyn Any + Send>, CommandError>;
// }

use std::{any::{Any, TypeId}, collections::HashMap, error::Error, sync::Arc};

use async_trait::async_trait;

use crate::{commands::{CreateJobCommand, CreateJobCommandHandler, ImportFileCommand, ImportFileCommandHandler}, db::{DbContext, RepositoryFactoryTrait}};

pub type CommandError = Box<dyn Error + Send + Sync>;

#[async_trait]
pub trait CommandHandler<C>: Send + Sync {
    async fn handle(&self, dependencies: Arc<CommandDependencies>, command: C) -> Result<Box<dyn Any + Send>, CommandError>;
}

#[async_trait]
pub trait CommandHandlerDyn: Send + Sync {
    async fn handle(&self, dependencies: Arc<CommandDependencies>, cmd: Box<dyn Any + Send>) -> Result<Box<dyn Any + Send>, CommandError>;
}


pub struct CommandHandlerAdapter<H, C> {
    inner: H,
    _phantom: std::marker::PhantomData<C>,
}

impl<H, C> CommandHandlerAdapter<H, C> {
    pub fn new(inner: H) -> Self {
        Self { inner, _phantom: Default::default() }
    }
}

#[async_trait]
impl<H, C> CommandHandlerDyn for CommandHandlerAdapter<H, C>
where
    H: CommandHandler<C> + Send + Sync,
    C: Send + Sync + 'static,
{
    async fn handle(&self, dependencies: Arc<CommandDependencies>, cmd: Box<dyn Any + Send>) -> Result<Box<dyn Any + Send>, CommandError> {
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

        Self { handlers, dependencies: deps }
    }

    pub async fn send(&self, cmd: Box<dyn Any + Send + Sync>) -> Result<(), CommandError> {
        let type_id = (*cmd).type_id();
        let handler = self.handlers
            .get(&type_id)
            .expect("Handler not found")
            .clone();

        handler.handle(Arc::clone(&self.dependencies), cmd).await?;
        Ok(())
    }
}