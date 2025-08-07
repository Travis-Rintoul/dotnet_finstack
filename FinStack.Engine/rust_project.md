
## Description of Key Components

- **`lib.rs`** – Entry point of the library. Re-exports modules for public use.
- **`models/`** – Holds domain structs (typically derived from `serde::Deserialize` and `serde::Serialize`).
- **`deserializers/`** – Functions to handle input formats like JSON or CSV.
- **`db/`** – Responsible for DB interaction. Includes optional `schema.rs` for Diesel.
- **`external/`** – Modules that handle external API interaction (e.g., HTTP clients).
- **`services/`** – Application logic that ties deserialization, DB, and external APIs together.
- **`config.rs`** – Loads and manages configuration (e.g., from environment variables).
- **`error.rs`** – Centralized error types, helpful for unified error handling across the lib.

## Jobs service

# Rust Job Service Overview

## 🧠 Purpose

This Rust service is designed as a high-performance engine to:
- Deserialize incoming data
- Perform complex business logic
- Make outbound API calls
- Interact directly with a shared PostgreSQL database
- Notify the .NET application upon job completion

## 📐 Architecture

- **.NET Layer**
    - Triggers "Jobs" via an `EngineService`
    - Persists job metadata in a shared `jobs` table
    - Optionally queries the database for results (no need to ask Rust)

- **Rust Layer**
    - Exposed via a library crate (`lib`)
    - Implements domain-specific job handlers (e.g., `create_orders`, `calculate_asset_values`)
    - Reports completion status and metadata back to the database
    - Optionally calls a .NET callback URL to signal job completion
-
---

## 📊 Database Schema

### `jobs`

| Column       | Type         | Description                        |
|--------------|--------------|------------------------------------|
| id           | UUID         | Unique job ID                      |
| job_type     | TEXT         | Type of job (e.g., CreateOrders)   |
| status       | TEXT         | `Pending`, `Success`, `Failed`     |
| started_at   | TIMESTAMPTZ  | When the job started               |
| finished_at  | TIMESTAMPTZ  | When it finished                   |
| parameters   | JSONB        | Input parameters                   |
| metadata     | JSONB        | Output results                     |
| error        | TEXT         | Error details if failed            |

### `job_results`

| Column      | Type   | Description                |
|-------------|--------|----------------------------|
| job_id      | UUID   | FK to `jobs.id`            |
| entity_type | TEXT   | e.g., "Order", "Asset"     |
| entity_id   | UUID   | ID of created entity       |

---

## ⚙️ Typical Flow

1. .NET inserts a row into `jobs` and triggers the Rust engine via FFI or an API.
2. Rust loads parameters from DB, dispatches the corresponding handler.
3. Handler performs logic, DB inserts, and populates `job_results`.
4. Rust updates job status (`Success` or `Failed`) and metadata.
5. Rust sends a completion callback to .NET (if configured).

---

## 🧩 Benefits

- High throughput (Rust handles time-critical logic)
- Database is the single source of truth (no duplication between Rust/.NET)
- Jobs are observable, auditable, and traceable
- Easily testable and extendable with new job types

---

## 🧪 Future Ideas

- Add retry/rollback support
- Schedule jobs via queue (e.g., Redis, NATS)
- Expose a CLI for ad-hoc job runs
- Export Prometheus metrics (e.g., job durations)

---

