# RyM Lubricentro - Service Management API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4)](https://docs.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A comprehensive RESTful API backend for managing a mechanic shop/lubricentro service business. Built with ASP.NET Core 8.0, following Clean Architecture principles with Domain-Driven Design (DDD).

## 📋 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Authentication & Authorization](#authentication--authorization)
- [Database](#database)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

### Core Functionality
- **User Management**: Complete user registration, authentication, and role-based access control
- **Order Management**: Create, schedule, and manage service orders with time slot availability
- **Mechanic Management**: Assign mechanics to orders and manage their service capabilities
- **Inventory Management**: Product/article management with categories
- **Financial Management**: Track income and expenses with comprehensive reporting
- **Marketing Tools**: Newsletter management, customer reviews, and subscription system
- **Comments System**: Order comments with reply functionality
- **Schedule Management**: Block/unblock dates for maintenance or holidays

### Security Features
- JWT-based authentication
- Role-based authorization (Admin, Mechanic, Customer)
- Password recovery via email
- Secure password hashing with ASP.NET Identity

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│  (Controllers, API Endpoints, Swagger) │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│      Application Layer                   │
│  (Services, DTOs, Validators, Mappers)   │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│         Domain Layer                     │
│  (Entities, Interfaces, Business Logic) │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│      Infrastructure Layer                │
│  (Repositories, Database, External APIs)│
└─────────────────────────────────────────┘
```

### Layer Responsibilities

- **Presentation**: HTTP request/response handling, API routing, Swagger documentation
- **Application**: Business logic orchestration, DTOs, validation, service interfaces
- **Domain**: Core business entities, domain interfaces, business rules
- **Infrastructure**: Data persistence, external service integrations, email services

## 🛠️ Technology Stack

### Core Technologies
- **.NET 8.0** - Modern C# framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 8.0** - ORM for database operations
- **SQL Server** - Relational database (Azure SQL or Local)

### Key Libraries
- **ASP.NET Identity** - User authentication and authorization
- **JWT Bearer Authentication** - Token-based security
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Swashbuckle (Swagger)** - API documentation
- **MailKit** - Email functionality
- **RazorEngine** - Email template rendering

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Azure SQL)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension
- [Git](https://git-scm.com/downloads)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/rym-lubricentro-api.git
cd rym-lubricentro-api
```

### 2. Configure Database Connection

Update the connection string in `Presentacion/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Context": "Your SQL Server connection string here",
    "DefaultConnection": "Data Source=localhost;Initial Catalog=ServicioRyM;Integrated Security=True;..."
  }
}
```

### 3. Run Database Migrations

```bash
cd Presentacion
dotnet ef database update --project ../Infraestructura
```

### 4. Configure Application Settings

Update `Presentacion/appsettings.json` with your settings:

- **JWT Settings**: Secret key, issuer, audience, expiration
- **SMTP Settings**: Email server configuration for notifications
- **CORS Origins**: Allowed frontend origins

### 5. Build and Run

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
cd Presentacion
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

## ⚙️ Configuration

### Environment Variables

For production, use environment variables or Azure Key Vault:

```bash
# JWT Configuration
JwtSettings__SecretKey=your-secret-key-here
JwtSettings__Issuer=your-issuer
JwtSettings__Audience=your-audience
JwtSettings__ExpiryInMinutes=60

# Database
ConnectionStrings__Context=your-connection-string

# SMTP
Smtp__Servidor=your-smtp-server
Smtp__Puerto=587
Smtp__Usuario=your-email
Smtp__Password=your-password
```

### CORS Configuration

Update CORS origins in `Startup.cs` to match your frontend:

```csharp
builder.WithOrigins(
    "http://localhost:3000",
    "https://your-production-domain.com"
)
```

## 📚 API Documentation

### Base URL

```
Development: http://localhost:5000/api
Production: https://your-api-domain.com/api
```

### Authentication

Most endpoints require JWT authentication. Include the token in the Authorization header:

```
Authorization: Bearer {your-jwt-token}
```

### API Endpoints Overview

#### 🔐 Authentication & Users (`/api/Usuarios`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/Registrar` | Register new user | No |
| POST | `/Login` | User login | No |
| POST | `/LoginAdmin` | Admin login | No |
| PUT | `/Actualizar/{cedula}` | Update user | Yes |
| PUT | `/Deseactivar` | Deactivate user | Yes |
| PUT | `/Activar` | Activate user | Yes |
| POST | `/AsignarRol` | Assign role to user | Yes |
| POST | `/RecuperarContraseña` | Request password reset | No |
| POST | `/restablecer-password` | Reset password | No |
| GET | `/ObtenerUsuario/{cedula}` | Get user by ID | Yes |
| GET | `/lista-usuarios` | List all users | Yes |

#### 📋 Orders (`/api/Ordenes`)

| Method | Endpoint | Description | Auth Required | Role |
|--------|----------|-------------|---------------|------|
| POST | `/crear` | Create new order | Yes | Customer |
| POST | `/crear-orden-admin` | Create order (admin) | Yes | Admin |
| GET | `/horas-disponibles` | Get available time slots | No | - |
| GET | `/listar-ordenes-usuario` | Get user's orders | Yes | Customer |
| GET | `/listar-todas-ordenes` | List all orders | Yes | Admin |
| GET | `/listar-ordenes-asignadas` | Get mechanic's orders | Yes | Mechanic |
| PUT | `/asignar-mecanico` | Assign mechanic to order | Yes | Admin |
| PUT | `/actualizar-estado` | Update order status | Yes | Admin/Mechanic |
| DELETE | `/Eliminarorden{id}` | Delete order | Yes | Admin |
| POST | `/bloquear-dia` | Block a date | Yes | Admin |
| POST | `/desbloquear-dia` | Unblock a date | Yes | Admin |
| GET | `/obtener-dias-bloqueados` | Get blocked dates | No | - |
| GET | `/lista-mecanicos` | Get available mechanics | No | - |

#### 🛠️ Mechanics (`/api/Mecanicos`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/obtener-mecanicos` | List all mechanics | No |
| POST | `/asignar-servicios/{usuarioId}` | Assign services to mechanic | Yes |

#### 📦 Articles (`/api/Articulo`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/Listar` | List all articles | No |
| GET | `/ListarPorCategoria/{categoriaId}` | List by category | No |
| GET | `/Obtener{id}` | Get article by ID | No |
| POST | `/Agregar` | Create article | Yes |
| PUT | `/Actualizar{id}` | Update article | Yes |
| DELETE | `/Eliminar{id}` | Delete article | Yes |

#### 📁 Categories (`/api/Categoria`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/Listar` | List all categories | No |
| GET | `/Obtener/{id}` | Get category by ID | No |

#### 💬 Comments (`/api/Comentarios`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/crear` | Create comment | Yes |
| PUT | `/editar/{id}` | Edit comment | Yes |
| DELETE | `/eliminar/{id}` | Delete comment | Yes |
| GET | `/obtenercomentariosusuario/{usuarioId}` | Get user comments | No |
| GET | `/obtenercomentarios` | Get all comments | No |
| POST | `/responder/{id}` | Reply to comment | Yes |

#### 💰 Financial (`/api/Ingreso`, `/api/Egreso`)

**Income Endpoints:**
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/Listar` | List all income records | Yes |
| GET | `/Obtener/{id}` | Get income by ID | Yes |
| POST | `/Crear` | Create income record | Yes |
| PUT | `/Actualizar/{id}` | Update income | Yes |
| DELETE | `/Eliminar/{id}` | Delete income | Yes |

**Expense Endpoints:**
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/Listar` | List all expense records | Yes |
| GET | `/Obtener/{id}` | Get expense by ID | Yes |
| POST | `/Crear` | Create expense record | Yes |
| PUT | `/Actualizar/{id}` | Update expense | Yes |
| DELETE | `/Eliminar/{id}` | Delete expense | Yes |

#### 📊 Reports (`/api/ReporteFinanciero`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/?fechaInicio={date}&fechaFin={date}` | Get financial report | Yes |

#### 📧 Marketing (`/api/Marketing`)

| Method | Endpoint | Description | Auth Required | Role |
|--------|----------|-------------|---------------|------|
| POST | `/CrearBoletin` | Create newsletter | Yes | Admin |
| GET | `/ObtenerBoletines` | List newsletters | No | - |
| PUT | `/ModificarBoletin` | Update newsletter | Yes | Admin |
| DELETE | `/EliminarBoletin/{id}` | Delete newsletter | Yes | Admin |
| POST | `/CrearResena` | Create review | No | - |
| GET | `/ObtenerResenas` | List reviews | No | - |
| PUT | `/ModificarResena/{id}` | Update review | Yes | - |
| DELETE | `/EliminarResena/{id}` | Delete review | Yes | - |
| POST | `/CrearSuscripcion` | Subscribe to newsletter | No | - |
| GET | `/ObtenerSuscripciones` | List subscriptions | Yes | Admin |
| PUT | `/ModificarSuscripcion/{id}` | Update subscription | Yes | Admin |
| DELETE | `/EliminarSuscripcion/{id}` | Delete subscription | Yes | Admin |

For detailed API documentation with request/response examples, see [API_DOCUMENTATION.md](API_DOCUMENTATION.md).

## 📁 Project Structure

```
rym-lubricentro-api/
│
├── Aplicacion/                    # Application Layer
│   ├── Interfaces/                # Service interfaces
│   ├── Servicios/                 # Business logic services
│   ├── Usuarios/
│   │   ├── Dtos/                  # Data Transfer Objects
│   │   ├── Mapeadores/            # AutoMapper profiles
│   │   └── Validadores/           # FluentValidation validators
│   ├── Ordenes/                   # Order-related DTOs
│   ├── Extensiones/               # Dependency injection extensions
│   └── Convertidores/             # JSON converters
│
├── Dominio/                       # Domain Layer
│   ├── Entidades/                 # Domain entities
│   ├── Interfaces/                # Domain interfaces
│   ├── Repositorios/              # Repository interfaces
│   └── Comun/                     # Common domain objects
│
├── Infraestructura/               # Infrastructure Layer
│   ├── DataBase/                  # EF Core DbContext
│   │   ├── Migrations/            # Database migrations
│   │   └── Seeder/                # Data seeders
│   ├── Usuarios/                  # User repository implementation
│   ├── Orden/                     # Order repository
│   ├── Articulos/                 # Article repository
│   ├── Categorias/                # Category repository
│   ├── Mecanicos/                 # Mechanic repository
│   ├── Marketing/                 # Marketing repositories
│   ├── Ingresos/                  # Income repository
│   ├── Egresos/                   # Expense repository
│   ├── Comentarios/               # Comment repository
│   ├── Servicios/                 # Service implementations
│   └── Extensiones/               # DI configuration
│
└── Presentacion/                  # Presentation Layer
    ├── Controllers/               # API controllers
    ├── Program.cs                 # Application entry point
    ├── Startup.cs                 # Startup configuration
    ├── appsettings.json           # Configuration
    └── wwwroot/                   # Static files
```

## 🔐 Authentication & Authorization

### User Registration

```http
POST /api/Usuarios/Registrar
Content-Type: application/json

{
  "correo": "user@example.com",
  "contraseña": "SecurePassword123!",
  "nombre": "John",
  "apellidos": "Doe",
  "cedula": "123456789"
}
```

### User Login

```http
POST /api/Usuarios/Login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Using the Token

Include the token in subsequent requests:

```http
GET /api/Ordenes/listar-ordenes-usuario
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Roles

- **Admin**: Full system access
- **Mecanico**: Can view assigned orders, update order status
- **Customer**: Can create orders, view own orders, manage profile

## 🗄️ Database

### Entity Relationships

- **Usuario** (1) ←→ (N) **Orden**: One user can have many orders
- **Orden** (N) ←→ (1) **Servicio**: Many orders for one service
- **Orden** (N) ←→ (1) **Mecanico**: Many orders assigned to one mechanic
- **Mecanico** (N) ←→ (N) **Servicio**: Many-to-many relationship
- **Articulo** (N) ←→ (1) **Categoria**: Many articles in one category
- **Comentario** (N) ←→ (1) **Orden**: Many comments per order

### Initial Data

The application seeds initial data on startup:
- **Roles**: Admin, Mecanico, Customer
- **Services**: Predefined service types
- **Categories**: Product categories

### Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName --project Infraestructura --startup-project Presentacion
```

Apply migrations:
```bash
dotnet ef database update --project Infraestructura --startup-project Presentacion
```

## 🧪 Testing

### Manual Testing with Swagger

1. Navigate to `/swagger` when the application is running
2. Use the "Authorize" button to add your JWT token
3. Test endpoints directly from the Swagger UI

### Postman Collection

Import the API endpoints into Postman for testing. See [API_DOCUMENTATION.md](API_DOCUMENTATION.md) for detailed examples.

## 🚢 Deployment

### Azure App Service

1. Create an Azure App Service
2. Configure connection strings in Application Settings
3. Set environment variables for sensitive data
4. Deploy via Visual Studio or Azure DevOps

### Docker (Optional)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Presentacion/Presentacion.csproj", "Presentacion/"]
# ... copy other projects
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentacion.dll"]
```

## 🤝 Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

### Development Workflow

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Write unit tests for new features

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Your Name** - *Initial work* - [YourGitHub](https://github.com/yourusername)

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Entity Framework Core for robust ORM capabilities
- All contributors and open-source libraries used

## 📞 Support

For support, email support@lubricentrorym.com or open an issue in the repository.

---

**Note**: This is a backend API. Ensure you have a compatible frontend application or use tools like Postman/Swagger to interact with the API.

