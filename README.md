
## 🖥️ Backend - ASP.NET Core Web API

The backend of Fintrack is a fully self-developed **ASP.NET Core Web API** application, responsible for managing authentication, portfolios, stock/crypto data, and user comments.

### 📆 Project Structure Highlights

| Folder        | Purpose |
|---------------|---------|
| `Controllers/` | API endpoints (Login, Register, Portfolio, Comments) |
| `Dtos/`        | Data Transfer Objects for safe and structured communication |
| `Interfaces/`  | Service and repository contracts for DI |
| `Repository/`  | Data access layer (Entity Framework + LINQ queries) |
| `Service/`     | Business logic and service layer implementations |
| `Mappers/`     | AutoMapper profiles for model-to-dto mapping |
| `Models/`      | Entity classes (User, Stock, Coin, Portfolio, etc.) |
| `Data/`        | DB Context and seeding logic |
| `Helpers/`     | Utility classes (Token generator, validators) |

### 🔐 Authentication

- JWT-based secure token generation  
- Role and claim-based route protection  
- Login/Register endpoint validation with `ModelState`

### 🚪 API Usage

- Swagger support enabled for testing endpoints  
- Example request/response formats available in `api.http`

### 🗃️ Database

- SQL Server with Entity Framework Core  
- Migrations and seeding implemented  
- Supports relationships like:
  - One-to-many (User → Portfolio)
  - Many-to-many (Users ↔ Stocks through Portfolios)
