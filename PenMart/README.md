# PenMart — ASP.NET Core MVC E-Commerce Sample (.NET 8)

PenMart is a work-in-progress e-commerce sample built with ASP.NET Core MVC.  
It demonstrates authentication with Identity, product browsing, a shopping cart workflow, and an admin area for managing products.

> Status: Work in Progress (portfolio / learning project)

## Implemented Features
- Category-based product listing
- Product details page (with images)
- Authentication (Register / Login / Logout) using ASP.NET Core Identity
- Shopping cart: add / remove / view items
- Admin panel (Razor Pages): product CRUD (create / edit / delete)

## Tech Stack
- .NET 8 (ASP.NET Core MVC)
- Entity Framework Core 8 (Code First + Migrations)
- SQL Server (local)
- ASP.NET Core Identity
- Bootstrap (RTL UI)

## Getting Started (Run Locally)

### Prerequisites
- .NET SDK 8
- SQL Server installed locally

### 1) Clone
```bash
git clone https://github.com/Aydashojaei/PenMart
cd <PenMart>


2) Configure database connection

Open appsettings.json and make sure the connection string is correct for your local SQL Server:

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=PenMart_DB;Integrated Security=True;TrustServerCertificate=True"
  }
}

Note: TrustServerCertificate=True is recommended for local development only.

3) Apply migrations:

dotnet ef database update

4) Run:

dotnet run --project PenMart/PenMart.csproj

## Notes
- Originally built on ASP.NET Core 3.1 and upgraded to .NET 8.

## Roadmap
- Secure admin area (Roles/Authorization)
- Complete checkout flow (finalize order)
- Add payment gateway integration
- Improve validations and error handling
- Add screenshots and documentation

License
This repository is provided as a learning / portfolio sample.



