use std::{error::Error, sync::Arc};

use crate::services::{
    commands_service::CommandDependencies,
    query_service::QueryTrait,
};

async fn dispatch_query<Q>(
    query: &Q,
    dependencies: &CommandDependencies,
) -> Result<Q::Response, Box<dyn Error + Send + Sync>>
where
    Q: QueryTrait,
{
    query.execute(dependencies).await
}

pub struct QueryRouter {
    services: Arc<CommandDependencies>,
}

impl QueryRouter {
    pub fn new(services: Arc<CommandDependencies>) -> Self {
        Self { services }
    }

    pub async fn send<Q>(&self, command: Q) -> Result<Q::Response, Box<dyn Error + Send + Sync>>
    where
        Q: QueryTrait,
    {
        command.execute(&Arc::clone(&self.services)).await
    }
}
