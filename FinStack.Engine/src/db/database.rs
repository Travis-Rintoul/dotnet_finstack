use sqlx::{PgPool};

use crate::db::pool::pool_connect;

pub struct DbContext {
    pub pool: PgPool,
}

impl DbContext {
    pub async fn connect() -> Result<Self, sqlx::Error> {
        let global_pool = pool_connect().await?;
        let pool = global_pool.clone();
        Ok(Self { pool })
    }
}