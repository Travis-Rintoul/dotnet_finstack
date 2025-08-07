use sqlx::PgPool;
use tokio::sync::OnceCell;

static DB_POOL: OnceCell<PgPool> = OnceCell::const_new();

pub async fn pool_connect() -> Result<&'static PgPool, sqlx::Error> {
    DB_POOL
        .get_or_try_init(|| async {
            PgPool::connect("postgresql://postgres:postgres@localhost:5432/finstack_db").await
        })
        .await
}