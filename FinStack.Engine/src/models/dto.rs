// use chrono::{DateTime, Utc};
// use sqlx::{Encode, FromRow};
// use uuid::Uuid;

// #[derive(Debug, FromRow)]
// pub struct UserDto{
//     pub id: i32,
//     pub email: String,
//     pub name: String,
// }

// #[derive(Debug, Encode, FromRow)]
// pub struct JobDto{
//     pub id: i32,
//     pub guid: Uuid,
//     pub job_code: String,
//     pub elapsed: i64,
//     pub success: bool,
//     pub start_time: DateTime<Utc>,
//     pub finish_time: DateTime<Utc>,
//     pub message: String,
// }

