# 🗺️ FinWise Project Roadmap

## ✅ Phase 1: Foundation & MVP

> Build the core trading infrastructure and simple frontend for validation.

### 🧱 Core Setup
- [x] Initialize all core projects:
  - `FinWise.API`
  - `FinWise.Web`
  - `FinWise.Trader`
  - `FinWise.Common`
- [x] Set up FFI bridge between `.NET` and `Rust`
- [x] Create CI/CD build pipelines for each service

### 🔐 Authentication & Accounts
- [x] Basic JWT-based user authentication  
- [x] Account creation & login APIs  
- [ ] Role-based access control (admin, trader, viewer)

### ⚙️ Trading Core (Rust)
- [x] Basic trade engine function via FFI (e.g., `place_order`, `cancel_order`)  
- [ ] Order validation logic (limits, balances)  
- [ ] Portfolio tracking (in-memory for now)
- [x] Testing framework layed out

### 🌐 API Development
- [ ] Define and expose REST endpoints:
  - `/orders`
  - `/portfolio`
  - `/account`
- [ ] Integrate Rust trade engine via P/Invoke or `DllImport`

### 💻 Web UI MVP
- [ ] Login/register screens  
- [ ] Submit order (buy/sell) form  
- [ ] Portfolio display table  

---

## 🚀 Phase 2: Strategy & Simulation

> Enable testing of strategies and simulate market conditions.

### 🧪 Simulator (Optional)
- [ ] Build `FinWise.Simulator` module to:
  - Simulate historical trades
  - Replay market data
  - Support strategy testing hooks

### 📉 Strategy Engine (Optional in Rust or C#)
- [ ] Basic rule-based strategy interface (e.g., “Buy low, sell high”)  
- [ ] Connect strategies to simulator

### 🧰 Web Enhancements
- [ ] Strategy config page  
- [ ] Historical trade logs  
- [ ] Order book visualization (static)  

---

## 🤖 Phase 3: Machine Learning & Analytics

> Add intelligence to the platform via data analysis and prediction models.

### 🧠 FinWise.ML Module
- [ ] Predictive model for market direction  
- [ ] Risk assessment models per user  
- [ ] Model hosting and inference API (via Python or ML.NET)

### 📊 Analytics UI
- [ ] Predictions dashboard  
- [ ] Portfolio performance charts  
- [ ] Risk exposure heatmaps  

---

## 🌍 Phase 4: Real-Time & Scaling

> Transition from MVP to a scalable, near-real-time platform.

### ⚡ Performance & Real-Time
- [ ] Replace HTTP polling with SignalR/WebSocket live feeds  
- [ ] Refactor Rust engine to async (tokio) and shared memory (optional)

### 📈 Market Data Feeds
- [ ] Connect to a mock or public market data provider  
- [ ] Stream prices to UI  

### ☁️ Infrastructure
- [ ] Dockerize all services  
- [ ] Deploy with Kubernetes or Azure App Services  
- [ ] Centralized logging (e.g., Serilog + Elastic)  

---

## 🔒 Phase 5: Security & Productionization

> Harden the system and get it ready for users.

### 🔐 Security
- [ ] MFA for sensitive actions  
- [ ] Audit logging of trade actions  
- [ ] Secure secret management  

### 📜 Compliance & Monitoring
- [ ] Activity logs with timestamps  
- [ ] Trade traceability for audit  
- [ ] Monitoring dashboards (Grafana/Prometheus)  

---

## 🏁 Stretch Goals

- [ ] Mobile UI (via Angular PWA or separate app)  
- [ ] Plugin system for third-party strategies  
- [ ] Copy-trading or social signals  
