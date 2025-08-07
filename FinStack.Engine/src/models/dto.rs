use chrono::{DateTime, Utc};
use sqlx::{Encode, FromRow};
use uuid::Uuid;

#[derive(Debug, FromRow)]
pub struct UserDto{
    pub id: i32,
    pub email: String,
    pub name: String,
}

#[derive(Debug, Encode, FromRow)]
pub struct JobDto{
    pub id: i32,
    pub guid: Uuid,
    pub created_date: DateTime<Utc>,
}