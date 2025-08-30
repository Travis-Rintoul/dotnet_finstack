
use std::{ffi::CStr, fs::{File, OpenOptions}, path::PathBuf, sync::Once};

pub fn ptr_to_string(ptr: *const i8) -> Option<String> {
    if ptr.is_null() {
        return None;
    }

    unsafe { CStr::from_ptr(ptr).to_str().ok().map(|s| s.to_string()) }
}

static LOGGER_INIT: Once = Once::new();

pub fn setup_logger() -> Result<PathBuf, Box<dyn std::error::Error>> {
    let mut res: Result<PathBuf, Box<dyn std::error::Error>> = Ok(PathBuf::new());

    LOGGER_INIT.call_once(|| {
        let mut path = std::env::current_dir().unwrap_or_else(|_| ".".into());
        path.push("../../../output.log");

        let file = OpenOptions::new()
            .create(true)
            .append(true)
            .open(&path);

        let r = file.and_then(|f| {
            Ok(simplelog::CombinedLogger::init(vec![simplelog::WriteLogger::new(
                simplelog::LevelFilter::Info,
                simplelog::Config::default(),
                f,
            )]).map(|_| ()))
        });

        match r {
            Ok(_) => {
                log::info!("logger initialized at {:?}", path);
                res = Ok(path);
            }
            Err(e) => {
                // If someone else already set a logger, you can treat it as OK:
                // use the error type/text you get to decide. Here we surface it.
                res = Err(Box::new(e));
            }
        }
    });

    res
}