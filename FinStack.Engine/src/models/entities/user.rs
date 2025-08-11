use sqlx::FromRow;

#[derive(Debug)]
pub struct User {
    pub id: i32,
    pub email: String,
    pub name: String,
}

#[derive(Debug, FromRow)]
pub struct UserDto{
    pub id: i32,
    pub email: String,
    pub name: String,
}