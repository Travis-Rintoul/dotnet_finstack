use sqlx::PgPool;
use tokio::sync::OnceCell;

use crate::config::CONFIG;

static DB_POOL: OnceCell<PgPool> = OnceCell::const_new();

pub async fn pool_connect() -> Result<&'static PgPool, sqlx::Error> {
    DB_POOL
        .get_or_try_init(|| async {

            let config = CONFIG.get().unwrap();

            // config.database

            // PgPool::connect(  &format!(
            //     "postgresql://{}:{}@{}:{}/{}",
            //     config.user,
            //     config.password,
            //     config.host,
            //     config.port,
            //     config.database,
            // )).await

            PgPool::connect("postgresql://postgres:postgres@localhost:5432/finstack_api_test_db").await
        })
        .await
}