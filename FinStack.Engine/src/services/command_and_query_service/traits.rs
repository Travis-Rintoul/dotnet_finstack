use crate::services::command_and_query_service::CQRSDependencies;
use async_trait::async_trait;
use std::{error::Error, sync::Arc};

#[async_trait]
pub(crate) trait QueryTrait {
    type Response;
    async fn execute(
        &self,
        services: &CQRSDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>>;
}

#[async_trait]
pub(crate) trait CommandTrait: Send + Sync {
    async fn handle(
        &self,
        services: Arc<CQRSDependencies>,
    ) -> Result<(), Box<dyn Error + Send + Sync>>;
}