use chrono::{DateTime, Utc};
use sqlx::{Encode, FromRow};
use uuid::Uuid;

#[allow(dead_code)]
#[derive(Debug)]
pub struct Job {
    pub id: i32,
    pub guid: Uuid,
    pub job_code: String,
    pub elapsed: i64,
    pub success: bool,
    pub start_time: DateTime<Utc>,
    pub finish_time: DateTime<Utc>,
    pub message: String,
}

#[derive(Debug, Encode, FromRow)]
pub struct JobDto {
    pub id: i32,
    pub guid: Uuid,
    pub job_code: String,
    pub elapsed: i64,
    pub success: bool,
    pub start_time: DateTime<Utc>,
    pub finish_time: DateTime<Utc>,
    pub message: String,
}
