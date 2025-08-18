use async_trait::async_trait;
use uuid::Uuid;

use crate::models::JobDto;

pub type RepositoryError = Box<dyn std::error::Error + Send + Sync>;

#[allow(dead_code)]
#[async_trait]
pub trait JobsRepositoryTrait: Send + Sync  {
    async fn create(&self, entity: JobDto) -> Result<Uuid, RepositoryError>;
    async fn find_by_id(&self, id: u32) -> Option<JobDto>;
    async fn find_all(&self) -> Vec<JobDto>;
    async fn update(&self, entity: JobDto) -> Result<Uuid, RepositoryError>;
    async fn remove(&self, entity: JobDto) -> Result<(), RepositoryError>;
}