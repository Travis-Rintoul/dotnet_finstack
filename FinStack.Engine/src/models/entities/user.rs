use sqlx::FromRow;

#[allow(dead_code)]
#[derive(Debug)]
pub struct User {
    pub id: i32,
    pub email: String,
    pub name: String,
}

#[allow(dead_code)]
#[derive(Debug, FromRow)]
pub struct UserDto{
    pub id: i32,
    pub email: String,
    pub name: String,
}