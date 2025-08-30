use std::{error::Error, sync::Arc};

use async_trait::async_trait;

use crate::services::command_and_query_service::{
    CQRSDependencies,
    traits::{CommandTrait, QueryTrait},
};

pub struct CQRSDispatcher {
    services: Arc<CQRSDependencies>,
}

impl CQRSDispatcher {
    pub fn new(services: Arc<CQRSDependencies>) -> Self {
        Self { services }
    }

    pub async fn send_query<Q>(&self, query: Q) -> Result<Q::Response, Box<dyn Error + Send + Sync>>
    where
        Q: QueryTrait,
    {
        query.execute(&Arc::clone(&self.services)).await
    }

    pub async fn send_command<C>(&self, command: C) -> Result<(), Box<dyn Error + Send + Sync>>
    where
        C: CommandTrait,
    {
        command.handle(Arc::clone(&self.services)).await
    }
}

#[allow(dead_code)]
struct TestQuery;

#[async_trait]
impl QueryTrait for TestQuery {
    type Response = String;

    async fn execute(
        &self,
        _services: &CQRSDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>> {
        Ok("Hello World!".into())
    }
}

#[allow(dead_code)]
struct TestCommand {
    foo: bool,
}

#[async_trait]
impl CommandTrait for TestCommand {
    async fn handle(
        &self,
        _services: Arc<CQRSDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>> {
        if self.foo {
            Ok(())
        } else {
            Err("bad value".into())
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[tokio::test]
    async fn test_send_query() {
        let services = Arc::new(CQRSDependencies::mock());
        let dispatcher = CQRSDispatcher::new(services);
        let query = TestQuery;

        let result = dispatcher.send_query(query).await.unwrap();
        assert_eq!(result, "Hello World!");
    }

    #[tokio::test]
    async fn test_send_command_success() {
        let services = Arc::new(CQRSDependencies::mock());
        let dispatcher = CQRSDispatcher::new(services);
        let command = TestCommand { foo: true };

        let result = dispatcher.send_command(command).await;
        assert!(result.is_ok());
    }

    #[tokio::test]
    async fn test_send_command_failure() {
        let services = Arc::new(CQRSDependencies::mock());
        let dispatcher = CQRSDispatcher::new(services);
        let command = TestCommand { foo: false };

        let result = dispatcher.send_command(command).await;
        assert!(result.is_err());
    }
}
