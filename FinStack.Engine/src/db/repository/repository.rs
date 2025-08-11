use uuid::Uuid;

pub trait Entity {}

pub trait Repository<T, E: Entity> {
    async fn create(&self, entity: T) -> Result<Uuid, sqlx::Error>;
    async fn find_by_id(&self, id: u32) -> Option<T>;
    async fn find_all(&self, ) -> Vec<T>;
    async fn update(&self, entity: T) -> Result<Uuid, sqlx::Error>;
    async fn remove(&self, entity: T) -> Result<(), sqlx::Error>;
}

