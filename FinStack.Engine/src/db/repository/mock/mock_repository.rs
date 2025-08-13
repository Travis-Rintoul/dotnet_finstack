use uuid::Uuid;

use crate::db::repository::mock::MockMethod;

pub struct MockRepository<T> {
    pub create_method: MockMethod<T, Result<Uuid, String>>,
    pub find_by_id_method: MockMethod<T, Result<Uuid, String>>,
    pub find_all_method: MockMethod<(), Result<Uuid, String>>,
    pub update_method: MockMethod<T, Result<Uuid, String>>,
    pub remove_method: MockMethod<T, Result<Uuid, String>>,
}

impl<T> MockRepository<T> {
    pub fn new() -> Self {
        Self::default()
    }
}

impl<T> Default for MockRepository<T> {
    fn default() -> Self {
        Self {
            create_method: MockMethod::new(|_| todo!("create_method not initialized")),
            find_by_id_method: MockMethod::new(|_| todo!("find_by_id_method not initialized")),
            find_all_method: MockMethod::new(|_| todo!("find_all_method not initialized")),
            update_method: MockMethod::new(|_| todo!("update_method not initialized")),
            remove_method: MockMethod::new(|_| todo!("remove_method not initialized")),
        }
    }
}
