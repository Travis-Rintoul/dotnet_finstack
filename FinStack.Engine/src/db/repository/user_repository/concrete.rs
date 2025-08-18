use std::sync::Arc;

use async_trait::async_trait;

use crate::{
    db::{DbContext, UsersRepositoryTrait},
    models::UserDto,
};

#[allow(dead_code)]
pub struct UserRepository {
    pub ctx: Arc<DbContext>,
}

impl UserRepository {
    pub fn new(ctx: Arc<DbContext>) -> Self {
        Self { ctx }
    }
}

#[allow(dead_code)]
#[async_trait]
impl UsersRepositoryTrait for UserRepository {
    async fn find_by_id(&self, _id: u32) -> Option<UserDto> {
        None
    }

    async fn find_all(&self) -> Vec<UserDto> {
        let users = match sqlx::query_as::<_, UserDto>(
            r#"
            SELECT
                "Id" AS id,
                "Email",
                "UserName" AS user_name
            FROM "AspNetUsers"
            "#,
        )
        .fetch_all(&self.ctx.pool)
        .await {
            Ok(users) => users,
            Err(error) => {
                log::error!("{error}");
                vec![]
            }
        };

        users
    }
}
