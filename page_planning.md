# 🧭 Product Flow Plan (User Journey)

## 1. 👤 User Onboarding

- **Register / Login**
  - User signs up with email & password or via Google (MFA optional).
  - On successful login, redirect to **Dashboard**.

---

## 2. 📊 Dashboard (Landing Page Post-Login)

**Goal:** Give the user a high-level snapshot of their trading account.

**Must-have elements:**

- 📈 **Portfolio Value Graph** (last 7 days)
  - X-axis: Date (daily)
  - Y-axis: Total account value in AUD
  - Tooltip on hover: Show value & % change from previous day

- 💵 **Current Balance**
  - Available cash
  - Total invested
  - PnL today and all-time

- 🛒 **Open Positions**
  - Table or cards with:
    - Ticker
    - Quantity
    - Avg. Buy Price
    - Current Price
    - PnL (green/red badge)

- 🧠 **Recommendation Section** (if ML enabled)
  - Top 3 buy/sell suggestions for the day with brief reasoning

---

## 3. 📥 Funding Page

- Deposit / Withdraw screen
- Integrate with mock API or real broker later
- Show transaction history

---

## 4. 📈 Trading Page

- Buy / Sell UI
- Search for asset (by ticker or name)
- Real-time quote (pulled from the `FinWise.Trader`)
- Execute simulated or live trade
- Show confirmation toast

---

## 5. 🧪 Simulate Page (if `FinWise.Simulator` enabled)

- Backtest trading strategy
- Choose:
  - Start/end date
  - Strategy (dropdown)
  - Initial capital
- Show:
  - Performance graph
  - Win rate / PnL / Sharpe ratio

---

## 6. ⚙️ Settings/Profile

- Update password, MFA settings, email
- Toggle real vs simulated mode (if supported)
- API key management (future)

---

## 🧱 Dev Notes for Implementation

### Backend (`FinWise.API`):

- Auth endpoints (`/auth/register`, `/auth/login`)
- Portfolio endpoints (`/portfolio/value`, `/portfolio/positions`)
- Trade endpoints (`/trade/quote`, `/trade/execute`)
- User settings endpoints

### Frontend (`FinWise.Web`):

- Route guards for auth
- Use charts (e.g. Chart.js or ngx-charts)
- Tailwind or Material for styling
