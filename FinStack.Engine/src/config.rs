use std::sync::Once;

use once_cell::sync::OnceCell;
use serde::Deserialize;

#[derive(Deserialize, Debug, Clone)]
pub struct Config {
    pub host: String,
    pub user: String,
    pub password: String,
    pub database: String,
    pub port: String,
    pub enviroment: String, // "TEST" | "DEV" | "PROD"
}

pub static LOG_INIT: Once = Once::new();
pub static CONFIG: OnceCell<Config> = OnceCell::new();