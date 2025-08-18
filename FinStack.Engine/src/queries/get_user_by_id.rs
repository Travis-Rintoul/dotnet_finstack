use std::error::Error;

use async_trait::async_trait;
use uuid::Uuid;

use crate::{
    models::UserDto,
    services::{commands_service::CommandDependencies, query_service::QueryTrait},
};

pub struct GetUserByIdQuery {
}

#[async_trait]
impl QueryTrait for GetUserByIdQuery {
    type Response = Option<UserDto>;

    async fn execute(
        &self,
        services: &CommandDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>> {

        Ok(None)
    }
}
