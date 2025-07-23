# 📚 API Endpoint Plan for `FinWise.API`

---

## 🔐 Auth & Users

| Method | Route                 | Description              | Auth Required |
| ------ | --------------------- | ------------------------ | ------------- |
| POST   | `/auth/register`      | Register a new user      | ❌             |
| POST   | `/auth/login`         | Log in and receive JWT   | ❌             |
| GET    | `/auth/me`            | Get current user profile | ✅             |
| POST   | `/auth/refresh-token` | Refresh access token     | ✅ (refresh)   |
| POST   | `/auth/logout`        | Invalidate session/token | ✅             |

---

## 👤 Account & Profile

| Method | Route                | Description                   | Auth Required |
| ------ | -------------------- | ----------------------------- | ------------- |
| GET    | `/account`           | Get current account details   | ✅             |
| PUT    | `/account`           | Update user profile           | ✅             |
| GET    | `/account/portfolio` | Get user’s portfolio summary  | ✅             |
| GET    | `/account/balance`   | Get current available balance | ✅             |

---

## 💰 Orders

| Method | Route          | Description                   | Auth Required |
| ------ | -------------- | ----------------------------- | ------------- |
| POST   | `/orders`      | Submit a new order (buy/sell) | ✅             |
| GET    | `/orders`      | List all user's orders        | ✅             |
| GET    | `/orders/{id}` | Get order by ID               | ✅             |
| DELETE | `/orders/{id}` | Cancel a pending order        | ✅             |

---

## 📈 Market Data

| Method | Route               | Description                        | Auth Required |
| ------ | ------------------- | ---------------------------------- | ------------- |
| GET    | `/market/ticker`    | Get current market price of assets | ❌             |
| GET    | `/market/history`   | Get historical price data          | ❌             |
| GET    | `/market/orderbook` | Get current order book snapshot    | ❌             |

---

## 🧪 Simulator (if enabled)

| Method | Route                | Description                  | Auth Required |
| ------ | -------------------- | ---------------------------- | ------------- |
| POST   | `/simulator/start`   | Start a simulation run       | ✅             |
| POST   | `/simulator/step`    | Advance simulation by 1 tick | ✅             |
| GET    | `/simulator/results` | View results of simulation   | ✅             |

---

## 🧠 ML & Analytics (optional)

| Method | Route                | Description                       | Auth Required |
| ------ | -------------------- | --------------------------------- | ------------- |
| GET    | `/ml/predictions`    | Get current model predictions     | ✅             |
| GET    | `/ml/portfolio-risk` | Analyze risk of current portfolio | ✅             |

---

## 🧾 Admin (Optional)

| Method | Route           | Description                        | Auth Required |
| ------ | --------------- | ---------------------------------- | ------------- |
| GET    | `/admin/users`  | List all users (admin only)        | ✅ (admin)     |
| GET    | `/admin/trades` | View all trade history (audit log) | ✅ (admin)     |
