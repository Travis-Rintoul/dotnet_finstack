use std::sync::Arc;

use chrono::Utc;
use once_cell::sync::Lazy;
use tokio::runtime::Runtime;

use crate::{
    commands::CreateJobCommand, db::{DbContext, RepositoryFactory}, models::UserDto, queries::{GetUserByIdQuery, GetUsersQuery}, services::{commands_service::{Command, CommandDependencies, CommandRouter}, query_service::QueryRouter}, JobGuid
};

static TOKIO_RUNTIME: Lazy<Runtime> =
    Lazy::new(|| Runtime::new().expect("Failed to create Tokio runtime"));

pub fn schedule_job_and_run(command_name: String, command: Command) -> JobGuid {
    let job_guid = uuid::Uuid::new_v4();

    TOKIO_RUNTIME.spawn(async move {

        let Ok(ctx) = DbContext::connect().await else {
            log::error!("Unable to connect to DB");
            return;
        };

        let db_context = Arc::new(ctx);
        let repo_factory = RepositoryFactory::new(Arc::clone(&db_context));

        let services = Arc::new(CommandDependencies::new(db_context, Box::new(repo_factory)));
        let query_runner = QueryRouter::new(Arc::clone(&services));
        let command_runner = CommandRouter::new(Arc::clone(&services));

        let start_time = Utc::now();
        log::info!(
            "Job Started {} ({:?}) at {}",
            command_name,
            job_guid,
            start_time
        );

        let command_result = command_runner.send(command).await;

        let finish_time = Utc::now();
        let elapsed = finish_time - start_time;

        log::info!(
            "Job Finished {} ({:?}) at {} elapsed {:.3}",
            command_name,
            job_guid,
            finish_time,
            elapsed
        );

        let get_user_query = GetUsersQuery;

        let user_result: Result<Vec<UserDto>, _> = query_runner.send(get_user_query).await;
        match user_result {
            Ok(users) => {
                println!("Found user: {:?}", users);
            }
            Err(e) => {
                eprintln!("Query failed: {}", e);
            }
}

        let elapsed_us: i64 = elapsed.num_microseconds().unwrap_or(0);

        let (success, message) = match command_result {
            Ok(_) => (true, String::new()),
            Err(e) => (false, e.to_string()),
        };

        let create_job_cmd = CreateJobCommand {
            job_guid,
            job_code: command_name,
            elapsed: elapsed_us,
            success,
            start_time,
            finish_time,
            message,
        };

        let result = command_runner.send(create_job_cmd).await;
        if result.is_err() {
            log::error!("ERROR CREATING JOB");
        }
    });

    JobGuid(*job_guid.as_bytes())
}
