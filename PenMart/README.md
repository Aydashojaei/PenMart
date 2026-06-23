# 🖊️ PenMart — ASP.NET Core MVC E-Commerce App
A production-style e-commerce project demonstrating real-world ASP.NET Core architecture and patterns.

PenMart is an e-commerce web app for selling stationery products (pens, notebooks, art supplies), built with **ASP.NET Core MVC (.NET 8)** and **Entity Framework Core**. It has a full storefront (categories, product details, cart, checkout) plus a role-protected admin panel for managing products, categories, and orders.

Built as a hands-on project to practice the **Repository Pattern**, **ASP.NET Core Identity**, and **EF Core Code-First migrations** in a realistic, multi-layer MVC app.

> Status: Core storefront and admin workflows are functional end-to-end. Still a work in progress — see [Known Limitations](#-known-limitations) below.

---

## ✨ Features

**Storefront**
- Home page with category navigation
- Browse products by category, view product details (with image gallery)
- Shopping cart (add / remove / view), tied to the logged-in user
- Checkout flow (name, phone, address) → order finalization
- Order history ("My Orders") and order success page
- Register / Login / Logout via ASP.NET Core Identity (Persian validation messages)
- Contact Us page, RTL UI (Bootstrap RTL build)

**Admin Panel** (`[Authorize(Roles = "Admin")]`)
- Dashboard with totals (products, categories, orders, pending orders)
- Full CRUD for products (with main + extra image upload) and categories
- Order management: view, finalize, or delete orders
- Default admin account auto-seeded on first run (see [Demo Admin Account](#-demo-admin-account))

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC (.NET 8, upgraded from .NET Core 3.1)
- **ORM:** Entity Framework Core 8 (Code-First + Migrations)
- **Auth:** ASP.NET Core Identity (Roles, `UserManager`, `SignInManager`)
- **Database:** SQL Server
- **Frontend:** Razor Views, Bootstrap RTL, jQuery, jQuery Validation
- **Patterns:** Repository Pattern, Dependency Injection, View Components, ViewModels

---

## 🏗️ Architecture

```
Controllers → Repositories → EF Core DbContext → SQL Server
     ↓
ViewModels → Razor Views
```

- **Controllers** handle requests and use repository interfaces (`IProductRepository`, `ICategoryRepository`, `IAdminRepository`) instead of touching `DbContext` directly — except `CartController`, which still queries `DbContext` directly (see limitations).
- **Models** are split into EF entities (`Product`, `Category`, `Order`, etc.) and dedicated **ViewModels** so views never bind directly to entities.
- **Services/FileService** handles saving/deleting uploaded images.
- **Data/Seed/AdminSeeder** creates the `Admin` role and a default admin user on startup.
- **Migrations** show the schema evolving over time (Identity tables, order tables, image URL fixes, etc.) rather than one static build.

### Folder Structure
```
PenMart/
├── Controllers/      # Account, Home, Product, Cart, Admin
├── Data/
│   ├── Repositories/  # Data access behind interfaces
│   └── Seed/          # AdminSeeder
├── Migrations/        # EF Core migration history
├── Models/             # Entities + ViewModels
├── Pages/Admin/        # Razor Pages used in the admin area
├── Services/           # File upload/delete service
├── Views/               # Razor views
└── wwwroot/              # Images, CSS, JS, Bootstrap RTL
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET SDK 8](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full)

### Steps
```bash
git clone https://github.com/<YOUR_USERNAME>/<REPO_NAME>.git
cd <REPO_NAME>
```

Update the connection string in `appsettings.json` if needed:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=.;Initial Catalog=PenMart_DB;Integrated Security=True;TrustServerCertificate=True"
}
```

Apply migrations and run:
```bash
dotnet ef database update --project PenMart/PenMart.csproj
dotnet run --project PenMart/PenMart.csproj
```

Open the URL shown in the console, then log in as admin (see below) and visit `/Admin`.

---

## 🔑 Demo Admin Account

A demo admin account is seeded automatically on first run, using credentials from `appsettings.json`:

```json
"AdminSeed": {
  "Email": "admin@penmart.ir",
  "Password": "Admin@123456"
}
```

This is a **demo-only** account meant so anyone evaluating this project can test the admin panel without extra setup. In a real production app, these values would come from environment variables or a secrets manager (e.g. ASP.NET Core User Secrets) instead of a committed config file.

---

## ⚠️ Known Limitations

- `CartController` queries `DbContext` directly instead of going through a repository, unlike the rest of the controllers — inconsistent with the project's layering, and on my list to refactor.
- No automated tests yet.
- No pagination/search on product listings.
- No real payment gateway — checkout just finalizes the order.

---

## 🔭 Possible Next Steps

- Add a `ICartRepository` / `IOrderRepository` to clean up `CartController`
- Add unit tests for repositories
- Add pagination and search to product listings
- Add a real payment gateway integration

---

## 📄 License

This repository is provided as a learning / portfolio sample.