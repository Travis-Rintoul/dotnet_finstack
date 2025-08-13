use async_trait::async_trait;
use uuid::Uuid;

use crate::models::JobDto;

#[async_trait]
pub trait JobsRepositoryTrait: Send + Sync  {
    async fn create(&self, entity: &JobDto) -> Result<Uuid, sqlx::Error>;
    async fn find_by_id(&self, id: u32) -> Option<JobDto>;
    async fn find_all(&self) -> Vec<JobDto>;
    async fn update(&self, entity: &JobDto) -> Result<Uuid, sqlx::Error>;
    async fn remove(&self, entity: &JobDto) -> Result<(), sqlx::Error>;
}