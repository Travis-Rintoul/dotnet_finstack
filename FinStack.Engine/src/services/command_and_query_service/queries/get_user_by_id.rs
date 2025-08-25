use std::error::Error;
use async_trait::async_trait;

use crate::{
    models::UserDto,
    services::command_and_query_service::{CQRSDependencies, traits::QueryTrait},
};

pub struct GetUserByIdQuery;

#[async_trait]
impl QueryTrait for GetUserByIdQuery {
    type Response = Option<UserDto>;

    async fn execute(
        &self,
        _services: &CQRSDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>> {
        Ok(None)
    }
}
