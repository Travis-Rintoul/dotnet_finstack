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
        services: &CQRSDependencies,
    ) -> Result<Self::Response, Box<dyn Error + Send + Sync>> {
        let Some(repo_factory) = &services.repository_factory else {
            log::error!("Unable to initialize repository");
            return Err("".into());
        };

        let users_repository = repo_factory.create_users_repository();

        let a = users_repository.find_all().await;

        log::info!("{:#?}", a);

        Ok(vec![])
    }
}
