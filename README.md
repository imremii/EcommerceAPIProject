# Ecommerce API

A layered ASP.NET Core Web API for an e-commerce system with authentication, role-based authorization, product & category management, cart handling, order placement, image upload, pagination, and filtering.

## Architecture

The solution is organized into 4 projects:

- `Ecommerce.API` – API layer (controllers, startup, identity setup, seeding)
- `Ecommerce.BLL` – business logic layer (managers, DTOs, validators)
- `Ecommerce.DAL` – data access layer (EF Core context, repositories, unit of work, entities)
- `Ecommerce.Common` – shared abstractions (`GeneralResult`, pagination, filtering)

## Tech Stack

- `.NET 10` (`net10.0`)
- `ASP.NET Core Web API`
- `Entity Framework Core` + `SQL Server`
- `ASP.NET Core Identity`
- `JWT Bearer Authentication`
- `FluentValidation`
- `Scalar` + OpenAPI for API docs

## Main Features

- User registration and login with JWT token generation
- Role-based authorization with policies:
  - `AdminOnly`
  - `CustomerOnly`
  - `AdminOrCustomer`
- Category management (CRUD + image update)
- Product management (CRUD + image update)
- Product pagination + search/sorting/filtering
- Customer cart management (add/update/remove/get)
- Order placement from cart and order history retrieval
- Image upload support (`.png`, `.jpg`, `.jpeg`)
- Standardized API response wrapper using `GeneralResult<T>`

## API Modules

- `AuthController` → register/login
- `CategoryController` → categories + category image upload
- `ProductController` → products + product image upload
- `CartController` → customer cart operations
- `OrderController` → customer orders
- `ImageController` → generic image upload endpoint (admin)

## Getting Started

### Prerequisites

- .NET SDK 10
- SQL Server (or SQL Server Express)
- EF Core tools (optional but recommended):

```bash
dotnet tool install --global dotnet-ef
```

### 1) Clone the repository

```bash
git clone https://github.com/imremii/EcommerceAPIProject.git
cd EcommerceAPIProject
```

### 2) Configure settings

Update `Ecommerce.API/appsettings.json`:

- `ConnectionStrings:DefaultConnection`
- `JwtSettings:SecretKey`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`
- `JwtSettings:ExpiryMinutes`

> Keep your JWT secret private in real deployments (use environment variables or user secrets).

### 3) Apply database migrations

Run from repository root:

```bash
dotnet ef database update --project Ecommerce.DAL --startup-project Ecommerce.API
```

### 4) Run the API

```bash
dotnet run --project Ecommerce.API
```

By default (development profile), the app runs on:

- `https://localhost:7059`
- `http://localhost:5161`

## API Documentation

In development mode, OpenAPI and Scalar are enabled:

- OpenAPI JSON: `https://localhost:7059/openapi/v1.json`
- Scalar UI: `https://localhost:7059/scalar`

## Authentication & Seeded Admin

On startup, roles and a default admin account are seeded:

- Roles: `Admin`, `Customer`
- Admin email: `admin@ecommerce.com`
- Admin password: `Admin@12345`

Use `POST /api/Auth/login` to get a JWT token, then pass it in:

```http
Authorization: Bearer <token>
```

## Request Notes

### Product listing query parameters

`GET /api/Product` supports:

- Pagination: `PageNumber`, `PageSize`
- Filtering: `MinCount`, `MaxCount`, `MinPrice`, `MaxPrice`, `Search`
- Sorting: `SortBy`, `SortDescending`

### Image uploads

Image upload endpoints expect `multipart/form-data` with form field name:

- `File`

Accepted extensions: `.png`, `.jpg`, `.jpeg`

## Demo Video
https://player.cloudinary.com/embed/?cloud_name=dmvrf2rns&public_id=Ecommerce_APIs_Testing_fx6ujz
This is a video of a postman collection for testing the APIs.

## Project Status

This project already includes core e-commerce API building blocks and is a good base for extending into payments, inventory workflows, shipping integration, and admin dashboards.
