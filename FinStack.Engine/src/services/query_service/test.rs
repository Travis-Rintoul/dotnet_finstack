use crate::{models::UserDto, services::commands_service::CommandDependencies};
use async_trait::async_trait;
use std::error::Error;
use uuid::Uuid;

#[async_trait]
pub trait QueryTrait {
    type Response;
    async fn execute(
        &self,
        services: &CommandDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>>;
}

struct GetUserByIdQuery {
    id: Uuid,
}

#[async_trait]
impl QueryTrait for GetUserByIdQuery {
    type Response = Option<UserDto>;

    async fn execute(
        &self,
        dependencies: &CommandDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>> {
        Ok(None)
    }
}
