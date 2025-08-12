use crate::{db::{DbContext, RepositoryFactoryTrait}, services::command_router::{CommandDependencies, CommandRouter}};

pub struct CommandRequestBuilder {
    pub dependencies: CommandDependencies
}

impl CommandRequestBuilder {
    pub fn new() -> Self {
        Self {
            dependencies: CommandDependencies::default(),
        }
    }

    pub fn add_db_context(mut self, db: DbContext) -> Self {
        self.dependencies.db = Some(db);
        self
    }

    pub fn add_repository_factory(mut self, repository_factory: Box<dyn RepositoryFactoryTrait>) -> Self {
        self.dependencies.repository_factory = Some(repository_factory);
        self
    }

    pub fn build(self) -> CommandRouter {
        CommandRouter::new(self.dependencies)
    }
}