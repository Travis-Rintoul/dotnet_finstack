use async_trait::async_trait;
use uuid::Uuid;

use crate::{
    db::{repository::mock::MockMethod, RepositoryError, UsersRepositoryTrait},
    models::UserDto,
};


#[allow(dead_code)]
#[derive(Clone)]
pub struct MockUserRepository {
    pub create_method: MockMethod<UserDto, Result<Uuid, RepositoryError>>,
    pub update_method: MockMethod<UserDto, Result<Uuid, RepositoryError>>,
    pub remove_method: MockMethod<UserDto, Result<(), RepositoryError>>,
    pub find_by_id_method: MockMethod<u32, Option<UserDto>>,
    pub find_all_method: MockMethod<(), Vec<UserDto>>,
}

#[allow(dead_code)]
impl MockUserRepository {
    pub fn new() -> Self {
        Self::default()
    }
}

impl Default for MockUserRepository {
    fn default() -> Self {
        Self {
            create_method: MockMethod::new(|_| todo!()),
            update_method: MockMethod::new(|_| todo!()),
            remove_method: MockMethod::new(|_| todo!()),
            find_by_id_method: MockMethod::new(|_| None),
            find_all_method: MockMethod::new(|_| vec![]),
        }
    }
}

#[async_trait]
impl UsersRepositoryTrait for MockUserRepository {
    async fn find_by_id(&self, id: u32) -> Option<UserDto> {
        self.find_by_id_method.call(id)
    }

    async fn find_all(&self) -> Vec<UserDto> {
        self.find_all_method.call(())
    }
}
