use sqlx::Error;

use crate::{db::DbContext, models::JobDto};

pub async fn create_job(job: JobDto) -> Result<u64, Error> {
    let db = DbContext::connect().await?;
    let sql = r#"
        INSERT INTO "Jobs" 
            ("Guid", "JobType", "Success", "ElapsedMs", "StartTime", "FinishTime", "Message")
            VALUES 
            ($1, $2, $3, $4, $5, $6, $7)
    "#;

    let query = sqlx::query(sql)
        .bind(job.guid)
        .bind(job.job_code)
        .bind(job.success)
        .bind(job.elapsed)
        .bind(job.start_time)
        .bind(job.finish_time)
        .bind(job.message);

    let query_result = query    
        .execute(&db.pool)
        .await;

    return match query_result {
        Ok(result) => Ok(result.rows_affected()),
        Err(e) => Err(e)
    }
}
