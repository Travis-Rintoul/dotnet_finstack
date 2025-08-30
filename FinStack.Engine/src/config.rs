use once_cell::sync::OnceCell;
use serde::Deserialize;

#[derive(Deserialize, Debug, Clone)]
pub struct Config {
    pub mode: String, // "TEST" | "DEV" | "PROD"
    pub connection_string: String,
}

pub static CONFIG: OnceCell<Config> = OnceCell::new();