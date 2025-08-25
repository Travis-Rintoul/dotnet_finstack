// use std::error::Error;
// use async_trait::async_trait;
// use crate::{db::DbContext, services::mediator::Handler};

// pub struct TestCommand {
//     pub file_path: String,
// }

// pub struct TestCommandHandler {
//     db: DbContext,
// }

// impl TestCommandHandler {
//     pub fn new(&self, db_ctx: DbContext) -> Self {
//         TestCommandHandler {
//             db: db_ctx
//         }
//     }
// }

// #[async_trait::async_trait]
// pub trait TestHandler<C>: Send + Sync {
//     async fn test(&self, command: &C) -> Result<(), Box<dyn Error + Send + Sync>>;
// }

// #[async_trait]
// impl Handler<TestCommand> for TestCommandHandler {
//     async fn handle(&self, command: &TestCommand) -> Result<(), Box<dyn Error + Send + Sync>> {
//         Ok(())
//     }
// }