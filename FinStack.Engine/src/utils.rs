use std::{ffi::CStr, fs::File};

use std::sync::Once;
use log::LevelFilter;
use simplelog::{CombinedLogger, Config, WriteLogger};

pub fn ptr_to_string(ptr: *const i8) -> Option<String> {
    if ptr.is_null() {
        return None;
    }

    unsafe { CStr::from_ptr(ptr).to_str().ok().map(|s| s.to_string()) }
}


static LOGGER_INIT: Once = Once::new();

pub fn setup_logger() -> Result<(), Box<dyn std::error::Error>> {
    let mut result: Result<(), Box<dyn std::error::Error>> = Ok(());

    LOGGER_INIT.call_once(|| {
        let r = CombinedLogger::init(vec![WriteLogger::new(
            LevelFilter::max(),
            Config::default(),
            File::create("output.log").unwrap(), // safe inside Once
        )]);

        if let Err(e) = r {
            result = Err(Box::new(e));
        }
    });

    result
}