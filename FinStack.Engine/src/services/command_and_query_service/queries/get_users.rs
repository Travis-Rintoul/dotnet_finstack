use crate::{
    models::UserDto,
    services::command_and_query_service::{CQRSDependencies, traits::QueryTrait},
};
use async_trait::async_trait;
use std::error::Error;

pub struct GetUsersQuery;

#[async_trait]
impl QueryTrait for GetUsersQuery {
    type Response = Vec<UserDto>;

    async fn execute(
        &self,
        dependencies: &CQRSDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>> {
        let users_repository = &dependencies.user_repository;

        let a = users_repository.find_all().await;

        log::info!("{:#?}", a);

        Ok(vec![])
    }
}
