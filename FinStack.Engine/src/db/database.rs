use sqlx::PgPool;
use tokio::sync::OnceCell;

use crate::config::CONFIG;

static DB_POOL: OnceCell<PgPool> = OnceCell::const_new();

pub struct DbContext {
    pub pool: PgPool,
}

impl DbContext {
    pub async fn connect() -> Result<Self, sqlx::Error> {
        let pool_ref = DB_POOL
            .get_or_try_init(|| async {
                let cfg = CONFIG.get().expect("CONFIG must be initialized before DbContext::connect");

                let url = format!(
                    "postgresql://{}:{}@{}:{}/{}",
                    cfg.user, cfg.password, cfg.host, cfg.port, cfg.database
                );

                PgPool::connect(&url).await
            })
            .await?;

        Ok(Self { pool: pool_ref.clone() })
    }
}
