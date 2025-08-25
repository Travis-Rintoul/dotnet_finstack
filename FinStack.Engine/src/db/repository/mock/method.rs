use std::sync::{Arc, Mutex};

pub struct MockMethod<I, O> {
    pub call_count: Arc<Mutex<u32>>,
    pub behavior: Arc<Mutex<Box<dyn Fn(I) -> O + Send + Sync>>>,
}

impl<I, O> MockMethod<I, O> {
    pub fn new(default: impl Fn(I) -> O + Send + Sync + 'static) -> Self {
        Self {
            call_count: Arc::new(Mutex::new(0)),
            behavior: Arc::new(Mutex::new(Box::new(default))),
        }
    }

    #[allow(dead_code)]
    pub fn set_behavior(&self, f: impl Fn(I) -> O + Send + Sync + 'static) {
        *self.behavior.lock().unwrap() = Box::new(f);
    }

    pub fn call(&self, input: I) -> O {
        *self.call_count.lock().unwrap() += 1;
        (self.behavior.lock().unwrap())(input)
    }
}

impl<I, O> Clone for MockMethod<I, O> {
    fn clone(&self) -> Self {
        Self {
            call_count: Arc::clone(&self.call_count),
            behavior: Arc::clone(&self.behavior),
        }
    }
}
