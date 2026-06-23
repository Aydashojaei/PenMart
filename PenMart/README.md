# 🖊️ PenMart — ASP.NET Core MVC E-Commerce Platform

PenMart is a full-stack e-commerce web application for selling stationery products (pens, notebooks, art supplies), built with **ASP.NET Core MVC (.NET 8)** and **Entity Framework Core**. It implements a complete storefront with category browsing, product details, a shopping cart, checkout flow, user authentication, and a role-protected admin panel for managing products, categories, and orders.

This project was built as a hands-on exercise in building a production-style multi-layer web application using the **Repository Pattern**, **ASP.NET Core Identity**, **Entity Framework Core Code-First Migrations**, and **MVC architecture**.

> Status: Actively developed portfolio project — core storefront and admin workflows are functional end-to-end.

---

## ✨ Implemented Features

### Storefront (Customer-Facing)
- **Home page** with banner and category-driven navigation
- **Category-based product listing** — browse products filtered by category
- **Product details page** displaying description, price, stock, and a main image plus up to two extra images
- **"Show all products"** catalog view
- **Contact Us** page
- **Shopping cart**: add to cart, remove items, view cart contents (tied to the logged-in user and persisted in the database)
- **Checkout flow**: collects full name, phone number, and delivery address, then finalizes the order
- **Order success page** and **"My Orders"** page showing a user's past finalized orders
- **User authentication**: Register, Login, Logout via ASP.NET Core Identity, with Persian-language validation messages and a "Remember Me" option
- **Reusable View Component** (`ProductCategoryComponent`) that renders the category sidebar/menu across pages
- **RTL (right-to-left) UI** using a customized Bootstrap RTL build, tailored for a Persian-language storefront

### Admin Panel (Role-Protected)
- Dedicated **Admin area**, secured with `[Authorize(Roles = "Admin")]`
- **Dashboard** showing total products, total categories, total orders, and pending (unfinalized) orders
- **Product management**: create, edit, delete products, including:
  - Main image upload (required) and up to 2 additional images
  - Automatic deletion of image files from disk when a product/image is replaced or removed
- **Category management**: create, edit, delete categories with an associated image, including a safeguard that blocks deletion of a category that still has products
- **Order management**: view all orders with computed totals, view order details, finalize an order, or delete an order
- **Default admin account auto-seeding** on application startup (creates an `Admin` role and a default admin user if none exists)

### Data & Backend
- **Code-First database schema** for Products, Items (price/stock), Categories, Product Images, Orders, and Order Details
- **Seeded sample data**: initial categories, items, products, and product images are pre-loaded via EF Core migrations
- **File upload service** (`IFileService` / `FileService`) abstracting image saving/deletion logic, used by both product and category management
- **Repository pattern** (`IProductRepository`, `ICategoryRepository`, `IAdminRepository`) separating data access from controllers
- Decimal precision configuration for prices, and explicit one-to-many relationship configuration via Fluent API

---

## 🛠️ Technologies Used

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 8), originally built on .NET Core 3.1 and upgraded |
| ORM | Entity Framework Core 8 (Code-First + Migrations) |
| Authentication | ASP.NET Core Identity (`IdentityUser`, Roles, `UserManager`, `SignInManager`) |
| Database | Microsoft SQL Server (via `Microsoft.EntityFrameworkCore.SqlServer`) |
| Frontend | Razor Views (`.cshtml`), Bootstrap (RTL fork — `bootstrap-v4-rtl`), jQuery, jQuery Validation / Unobtrusive Validation |
| Admin UI | Razor Pages (`Pages/Admin/...`) alongside MVC Controllers |
| Architecture Patterns | Repository Pattern, Dependency Injection, View Components, ViewModels (DTO-style separation from EF entities) |
| Other Packages | `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.VisualStudio.Web.CodeGeneration.Design`, `Microsoft.EntityFrameworkCore.Sqlite` (referenced) |

---

## 🏗️ Architecture & Project Structure

PenMart follows a classic **layered ASP.NET Core MVC architecture**:

```
Controllers  →  Repositories (Data Access)  →  EF Core DbContext  →  SQL Server
     ↓
  ViewModels  →  Razor Views
```

- **Controllers** (`AccountController`, `HomeController`, `ProductController`, `CartController`, `AdminController`) handle HTTP requests and delegate data access to repository interfaces injected via DI — controllers never talk to `DbContext` directly for core domain logic (cart/order handling in `CartController` is the one place raw `DbContext` queries are used directly).
- **Repositories** (`Data/Repositories/`) encapsulate all EF Core queries behind interfaces (`IProductRepository`, `ICategoryRepository`, `IAdminRepository`), making the data layer swappable and unit-testable.
- **Models** (`Models/`) contain both EF Core entities (`Product`, `Category`, `Item`, `Order`, `OrderDetail`, `ProductImage`, `ApplicationUser`) and dedicated **ViewModels** (`ProductViewModel`, `CheckoutViewModel`, `AdminProductViewModel`, etc.) used to shape data sent to views without exposing entities directly.
- **Services** (`Services/`) hold cross-cutting infrastructure logic — currently the file upload/delete service used by the admin panel.
- **Data/Seed** contains `AdminSeeder`, which runs at application startup to guarantee an `Admin` role and a default admin account always exist.
- **Components** contains a Razor **View Component** (`ProductCategoryComponent`) for rendering the category navigation independently of any single controller/view.
- **Migrations** track the schema's evolution over time (Identity tables, order tables, product image URL fixes, category images, etc.), reflecting iterative, real-world development rather than a single static schema.

### Folder Structure

```
PenMart/
├── Components/                 # Razor View Components (e.g., category sidebar)
├── Controllers/                # MVC Controllers (Account, Home, Product, Cart, Admin)
├── Data/
│   ├── PenMartContext.cs       # EF Core DbContext + Fluent API config + seed data
│   ├── Repositories/           # Repository interfaces + implementations
│   └── Seed/                   # AdminSeeder (default Admin role/user)
├── Migrations/                 # EF Core Code-First migrations history
├── Models/
│   ├── Admin/                  # Admin-area ViewModels (Product, Category, Order)
│   └── *.cs                    # Domain entities + customer-facing ViewModels
├── Pages/Admin/                # Razor Pages used inside the Admin area
├── Services/                   # File upload/delete abstraction (IFileService)
├── Views/                      # Razor Views (Account, Admin, Cart, Home, Product, Shared)
├── wwwroot/
│   ├── Images/                 # Product, category, and contact-page images
│   ├── css / js                # Site styles & scripts
│   ├── lib/                    # jQuery, jQuery Validation, Bootstrap
│   └── bootstrap-v4-rtl-master/# RTL-customized Bootstrap build
├── appsettings.json             # Connection string & logging configuration
├── Program.cs                   # Application entry point
└── Startup.cs                   # Service registration & middleware pipeline
```

---

## 🚀 Getting Started (Run Locally)

### Prerequisites
- [.NET SDK 8](https://dotnet.microsoft.com/download)
- Microsoft SQL Server (LocalDB, Express, or full SQL Server)
- (Optional) Visual Studio 2022 / VS Code

### 1. Clone the repository
```bash
git clone https://github.com/<YOUR_USERNAME>/<REPO_NAME>.git
cd <REPO_NAME>
```

### 2. Configure the database connection
Open `PenMart/appsettings.json` and update the connection string to match your local SQL Server instance:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=PenMart_DB;Integrated Security=True;TrustServerCertificate=True"
  }
}
```
> `TrustServerCertificate=True` is intended for **local development only**.

### 3. Apply EF Core migrations
```bash
dotnet ef database update --project PenMart/PenMart.csproj
```
This creates the `PenMart_DB` database, applies the full schema (Identity tables, products, categories, orders, etc.), and inserts the seeded sample categories/products.

### 4. Run the application
```bash
dotnet run --project PenMart/PenMart.csproj
```
Then open the URL shown in the console (e.g. `https://localhost:5001`).

### 5. Log in as Admin
On first run, `AdminSeeder` automatically creates a default admin account using the credentials defined in `appsettings.json` (see [Demo Admin Account](#-demo-admin-account) below).

Navigate to `/Admin` after logging in with this account to access the admin dashboard.

---

## 🔑 Demo Admin Account

For evaluation purposes, the application seeds a demo admin account on first run, using the credentials configured in `appsettings.json` under the `AdminSeed` section:

```json
"AdminSeed": {
  "Email": "admin@penmart.ir",
  "Password": "Admin@123456"
}
```

- **Email:** `admin@penmart.ir`
- **Password:** `Admin@123456`

> ⚠️ This account exists only for local/demo evaluation so reviewers and recruiters can test the admin panel without setting anything up. In any real deployment, change these values in `appsettings.json` (or override them via environment variables / a secrets store) before going live.

---

## 🗄️ Database Setup Notes

- The project uses **Code-First** EF Core migrations — the schema is fully defined in C# (`Models/` + `PenMartContext.OnModelCreating`) and the `Migrations/` folder, not designed in SQL Server Management Studio.
- Sample data (3 categories, 9 items, 9 products, and their images) is seeded directly through `HasData(...)` calls in `PenMartContext`, so a fresh database is immediately populated with browsable content.
- If you need to reset the database from scratch:
  ```bash
  dotnet ef database drop --project PenMart/PenMart.csproj
  dotnet ef database update --project PenMart/PenMart.csproj
  ```
- The `Microsoft.EntityFrameworkCore.Sqlite` package is also referenced in the project file, but the application is currently wired to **SQL Server** via `Startup.cs` (`options.UseSqlServer(...)`).

---

## 📸 Suggested Screenshots

When publishing this project to GitHub, consider capturing:
1. **Home page** — banner + category navigation
2. **Product listing by category**
3. **Product details page** — main image, gallery, price, "Add to Cart"
4. **Shopping cart page** with items and quantities
5. **Checkout form** (name, phone, address)
6. **Order success / "My Orders"** page
7. **Login and Register pages** (showing Persian validation messages)
8. **Admin dashboard** — totals for products/categories/orders
9. **Admin product list + Create/Edit Product form** (with image upload)
10. **Admin category list + Create/Edit Category form**
11. **Admin orders list + order detail view**

---

## 🔭 Future Improvements

- Add unit/integration tests for repositories and controllers
- Externalize the default admin credentials (e.g., via configuration or environment variables) instead of hard-coding them in `AdminSeeder`
- Add pagination and search/filtering to product listing pages
- Add real payment gateway integration to the checkout flow
- Add server-side image validation (file type/size limits) in `FileService`
- Introduce API endpoints (e.g., minimal API or separate Web API project) for a future SPA/mobile client
- Add caching for category and product listings
- Containerize the app with Docker and add CI/CD (build + EF migration check) via GitHub Actions
- Improve authorization granularity (e.g., distinguish "Admin" from future roles like "Editor")

---

## 🎯 Skills Demonstrated

- Building a multi-layer **ASP.NET Core MVC** application from scratch (Controllers, Views, ViewModels, Services, Repositories)
- Designing a **relational database schema** with EF Core Code-First and managing its evolution through **migrations**
- Implementing **authentication and role-based authorization** with ASP.NET Core Identity
- Applying the **Repository Pattern** and **Dependency Injection** to decouple data access from business/controller logic
- Handling **file uploads** (product/category images) safely, including cleanup of old files on update/delete
- Building a **shopping cart and order/checkout workflow** tied to the authenticated user
- Creating a secured **admin back office** with full CRUD over products, categories, and orders, mixing MVC Controllers and Razor Pages in the same application
- Working with **Razor View Components** for reusable, data-driven UI fragments
- Adapting a third-party CSS framework (Bootstrap) for an **RTL, Persian-language UI**
- Iterative schema evolution across many real migrations (adding tables, fixing image URL handling, adding Identity, adding order tables) — reflecting realistic, incremental software development rather than a single static build

---

## ⚠️ Known Limitations / Before Publishing to GitHub

A few things are worth addressing or being aware of before sharing this repository publicly:

1. **Demo admin credentials in configuration** — `appsettings.json` ships with a default `AdminSeed` email/password so reviewers can test the admin panel immediately (see [Demo Admin Account](#-demo-admin-account)). These are intentionally simple demo values — replace them before any real/production deployment.
2. **Connection string with `TrustServerCertificate=True`** — fine for local development, but should not be used as-is in any production environment.
3. **Build artifacts and IDE folders committed** — the uploaded project includes `bin/`, `obj/`, and `.vs/` folders, which should be excluded via `.gitignore` before pushing to GitHub (a `.gitignore` is present, so verify these folders aren't already tracked).
4. **Mixed languages in code comments and validation messages** — some code comments and all user-facing validation/error messages are in Persian, while identifiers and structure are in English. This is fine for a Persian-market storefront but worth mentioning explicitly here so reviewers aren't confused.
5. **Two ASP.NET Core target generations in build output** — the uploaded project contains compiled output for both `net8.0` and `netcoreapp3.1`, consistent with the documented upgrade from 3.1 to 8.0; only the `net8.0` output is relevant going forward.
6. **No automated tests** — there is currently no test project; adding one would strengthen the portfolio value of this repository.
7. **`CartController` bypasses the repository layer** for direct `DbContext` queries (unlike `ProductController`/`AdminController`), which is inconsistent with the rest of the codebase's layering — worth refactoring into an `ICartRepository`/`IOrderRepository` for consistency.

---

## 📦 Which Parts to Highlight in Your Resume / Portfolio

If you're using this project to show employers your skills, the most resume-relevant pieces are:
- **The Admin Panel** (full CRUD + role-based authorization) — demonstrates back-office/admin development skills, which map directly to real business applications
- **The Repository Pattern + DI setup** in `Startup.cs` and `Data/Repositories/` — shows you understand clean separation of concerns, not just "make it work" code
- **The EF Core migration history** — a real, evolving schema (not a single seed-and-done database) is a strong signal of iterative, professional development
- **The Identity + Roles implementation** — authentication/authorization is one of the most commonly assessed skills in interviews
- **The Cart → Checkout → Order finalization flow** — a complete, working business transaction flow end-to-end is more compelling than isolated CRUD screens

---

## 📄 License

This repository is provided as a learning / portfolio sample.