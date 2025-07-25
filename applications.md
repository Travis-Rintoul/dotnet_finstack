# 🧱 FinStack Project Structure & Responsibilities

This document outlines where to place different components in your FinStack project, focusing on separation of concerns and clean architecture principles.

---

## 📂 FinStack.API

**Responsibility:**  
- Entry point of the backend API.
- Hosts controllers and minimal logic.
- Responsible for routing and translating HTTP requests to application commands or queries.

**Examples:**
- `UserController.cs`
- `PortfolioController.cs`

---

## 📂 FinStack.Web

**Responsibility:**  
- Angular frontend (or other UI).
- Calls APIs and renders data to the user.
- Manages routing, UI state, and authentication token storage.

**Examples:**
- Angular components for dashboard, trades, portfolio
- Angular services that call `FinStack.API`

---

## 📂 FinStack.Application

**Responsibility:**  
- Contains the business logic as use-cases (CQRS).
- Implements application services, commands, queries, and orchestrates domain models.
- Contains interfaces that Infrastructure will implement.

**Examples:**
- `CreateTradeCommandHandler.cs`
- `GetPortfolioQueryHandler.cs`
- `ITradeExecutionService.cs`

---

## 📂 FinStack.Domain

**Responsibility:**  
- Pure business rules and entities.
- No knowledge of application or infrastructure layers.
- Includes domain services, value objects, and domain events.

**Examples:**
- `Trade.cs` (entity)
- `OrderStatus.cs` (enum or value object)
- `TradeExecutedDomainEvent.cs`

---

## 📂 FinStack.Infrastructure

**Responsibility:**  
- Implements interfaces from Application (e.g., repositories, services).
- Handles database access, file systems, external APIs, and logging.
- Should not contain business logic.

**Examples:**
- `EfCoreTradeRepository.cs`
- `PostgresPortfolioReader.cs`
- `ExternalPriceFeedClient.cs`

---

## 📂 FinStack.Engine

**Responsibility:**  
- High-performance logic written in Rust (via FFI).
- Used for things like simulations, quote evaluation, or strategy backtesting.
- Called from Infrastructure or directly through interop layers.

**Examples:**
- `lib.rs` – exposes `get_quote`, `simulate_strategy`, etc.
- Extern functions callable from C# using `DllImport`

---

## 📦 Where to Put Specific Logic

| Component Type              | Location                   |
|----------------------------|----------------------------|
| Entity (e.g., Trade, User) | `Domain/Entities`          |
| Enum / Value Object        | `Domain/ValueObjects`      |
| Business Rules             | `Domain/Services`          |
| DTOs                       | `Application/DTOs`         |
| Command / Query Handlers   | `Application/Handlers`     |
| Interfaces (e.g., IRepo)   | `Application/Interfaces`   |
| Repository Implementation  | `Infrastructure/Repositories` |
| API Controllers            | `API/Controllers`          |
| Angular Components         | `Web/src/app/components/`  |
| Angular Services           | `Web/src/app/services/`    |

---

## ✅ Summary

This architecture helps enforce clean separation of concerns:

- `Domain`: What your app *is*
- `Application`: What your app *does*
- `Infrastructure`: How your app *does it*
- `API`: How your app *is accessed*
- `Web`: What your app *looks like*
- `Engine`: Where the *performance-critical* logic lives
