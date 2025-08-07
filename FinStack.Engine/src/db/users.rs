use sqlx::Error;

use crate::{
    db::database, models::UserDto,
};

use database::DbContext;

pub async fn get_users() -> Result<Vec<UserDto>, Error> {
    let db = DbContext::connect().await?;
    let sql = r#"
        SELECT 
            "Id" as "id", 
            "Email" as "email", 
            "Name" as "name"
        FROM "Users"
        ORDER BY "Name"
    "#;

    let query = sqlx::query_as::<_, UserDto>(sql);

    match query.fetch_all(&db.pool).await {
        Ok(users) => Ok(users),
        Err(e) => Err(e),
    }
}
