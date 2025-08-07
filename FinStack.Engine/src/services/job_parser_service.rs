use std::ffi::CStr;

pub fn parse_job_code(job_code_ptr: *const i8) -> Option<String> {
    if job_code_ptr.is_null() {
        return None;
    }

    unsafe {
        return match CStr::from_ptr(job_code_ptr).to_str() {
            Ok(s) => Some(s.to_string()),
            Err(_) => None
        }
    };
}

pub fn parse_job_body(job_body_ptr: *const i8) -> Option<String> {
    if job_body_ptr.is_null() {
        return None;
    }

    unsafe {
        return match CStr::from_ptr(job_body_ptr).to_str() {
            Ok(s) => Some(s.to_string()),
            Err(_) => None
        }
    };
}