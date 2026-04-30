# POS + ERP Integration Module
---

## What I Built

A Retail POS prototype that demonstrates:

- Offline sale entry with local SQLite storage
- Inventory management with real-time stock deduction
- Sync mechanism from local POS to central ERP server
  

The system has two separate applications:

- **POS.Api** — ASP.NET Core Web API (backend, business logic, database)
- **POS.Web** — ASP.NET Core MVC (frontend, UI, API communication)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend API | ASP.NET Core 8 Web API |
| Frontend | ASP.NET Core 8 MVC (Razor Views) |
| Database | SQLite via Entity Framework Core |
| UI | Bootstrap 5 + Bootstrap Icons |
| Language | C# / .NET 8 |

---

## Project Structure

```
POS/
├── POS.Api/
│   ├── Controllers/
│   │   ├── ProductController.cs      
│   │   ├── SaleController.cs
│   │   └── SyncController.cs
│   ├
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── Product.cs
│   │   └── Sale.cs
│   ├── Database/
│   │   └── pos.db                     
│   └── Program.cs
│
└── POS.Web/
    ├── Controllers/
    │   ├── ProductController.cs
    │   ├── SaleController.cs
    │   └── SyncController.cs
    ├── Models/
    │   ├── Product.cs
    │   └── Sale.cs
    ├── Services/
    │   └── ApiService.cs              
    ├── Views/
    │   ├── Product/
    │   │   ├── Index.cshtml
    │   │   └── Create.cshtml
    │   ├── Sale/
    │   │   └── Index.cshtml
    │   └── Sync/
    │       └── Index.cshtml
    └── Program.cs
```

---

## How to Run

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code

---

### Step 1 — Run POS.Api (Backend)

API starts at: `https://localhost:7022`

> SQLite database (`pos.db`) is auto-created inside `POS.Api/Database/` on first run. No manual setup needed.

---

### Step 2 — Run POS.Web (Frontend)

Open the browser and go to the URL shown in your terminal.

---

### Step 3 — Test the Flow

1. **Product List** → click **Create** → add a product
2. **Go To Sales** → select product → fill quantity & price → **Save Sale**
   - Status shows `Pending`
3. **Go To Sync** → click **Sync Now**
   - Status changes to `Synced`

---

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Get all products |
| POST | `/api/products` | Create a product |
| GET | `/api/sales` | Get all sales |
| POST | `/api/sales` | Create a sale |
| POST | `/api/sync/sales` | Sync all pending sales |

---

## How the Sync Works

Every sale is saved to `pos.db` with `Status = "Pending"` — this simulates the POS working offline without a server connection.

When the user clicks **Sync Now**, the app calls `POST /api/sync/sales`. The API finds all `Pending` sales and marks them `Synced`.

```
[ Create Sale ]
      │
      ▼
[ Save to pos.db ]  →  Status = "Pending"
      │
      │   (user clicks Sync Now)
      ▼
[ POST /api/sync/sales ]
      │
      ▼
[ Find all Pending sales ]
      │
      ▼
[ Update Status = "Synced" ]
      │
      ▼
[ Save to pos.db ] ✓
```

**Idempotency:** The sync endpoint only processes `Pending` records. Running it multiple times will never duplicate or re-process an already synced sale.

**Retry on failure:** If a sale fails to sync, its status is set to `Failed`. These records are retried on the next sync attempt.

---

## Sale ID Format

I used a human-readable date-based format instead of a random GUID:

```
2026-04-30-01    →  First sale of April 30, 2026
2026-04-30-02    →  Second sale of the same day
2026-05-01-01    →  Resets to 01 on next day
```

This makes it easy to trace any sale by date without needing a lookup.

---

## Design Decisions

**Why SQLite?**
SQLite is file-based and needs zero server setup. It perfectly simulates a local POS database that works without internet. For production I would switch to SQL Server or PostgreSQL.

**Why separate POS.Api and POS.Web?**
Separating the API from the frontend means the backend can later serve a mobile app, Angular client, or any other consumer without any changes to the core logic.


**Why Status field (Pending / Synced / Failed)?**
Status is the foundation of the sync mechanism. It tells the system exactly which records need to be sent, which are done, and which need a retry. Without it, tracking sync state would require a separate table.

**Why date-based Sale ID?**
A format like `2026-04-30-01` tells you the date and sequence at a glance — much more useful in a retail environment.

---

## Limitations

- **No real browser offline mode** — the web app still needs the API running. True offline support would require IndexedDB or localStorage on the browser side with a background sync worker.
- **No authentication** — no login or user roles. A production system would need JWT-based auth with role separation (cashier, manager, admin).
- **No pagination** — all sales load at once. With large datasets this would need server-side paging.
- **Single outlet only** — designed for one POS terminal. Multi-outlet support would need a tenant/branch system with isolated data per outlet.
- **No queue-based sync** — sync runs synchronously. For high volume, a proper message queue (Hangfire, RabbitMQ) would be more reliable.

---

## If I Had More Time

- Add JWT authentication and role-based access
- Implement true offline mode with browser-side storage and background sync
- Add Hangfire for queue-based retry sync
- Build a dashboard with daily/weekly sales reports
- Support multiple outlets with branch-level data isolation
- Add unit tests for service and repository layers
