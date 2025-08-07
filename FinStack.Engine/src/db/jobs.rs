use sqlx::{query, Error};

use crate::{db::DbContext, models::JobDto};

pub async fn get_jobs() -> Result<Vec<JobDto>, Error> {
    let db = DbContext::connect().await?;
    let sql = r#"
        SELECT 
            "Id" as "id", 
            "Email" as "email", 
            "Name" as "name"
        FROM "Users"
        ORDER BY "Name"
    "#;

    let query = sqlx::query_as::<_, JobDto>(sql);

    match query.fetch_all(&db.pool).await {
        Ok(users) => Ok(users),
        Err(e) => Err(e),
    }
}

pub async fn create_job(job: JobDto) -> Result<u64, Error> {
    let db = DbContext::connect().await?;
    let sql = r#"
        INSERT INTO "Jobs" 
            ("Guid", "CreatedDate")
            VALUES 
            ($1, $2)
    "#;

    let query = sqlx::query(sql)
        .bind(job.guid)
        .bind(job.created_date);

    let query_result = query    
        .execute(&db.pool)
        .await;

    return match query_result {
        Ok(result) => Ok(result.rows_affected()),
        Err(e) => Err(e)
    }
}
