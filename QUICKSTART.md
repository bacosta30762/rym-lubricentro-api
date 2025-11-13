# Quick Start Guide

Get up and running with the RyM Lubricentro API in minutes.

## Prerequisites Checklist

- [ ] .NET 8.0 SDK installed
- [ ] SQL Server (LocalDB, Express, or Azure SQL)
- [ ] Git installed
- [ ] Code editor (Visual Studio, VS Code, or Rider)

## 5-Minute Setup

### Step 1: Clone and Navigate

```bash
git clone https://github.com/yourusername/rym-lubricentro-api.git
cd rym-lubricentro-api
```

### Step 2: Configure Database

Edit `Presentacion/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Context": "Server=localhost;Database=ServicioRyM;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

### Step 3: Run Migrations

```bash
cd Presentacion
dotnet ef database update --project ../Infraestructura
```

### Step 4: Run the Application

```bash
dotnet run
```

### Step 5: Access Swagger

Open your browser to:
```
https://localhost:5001/swagger
```

## First API Call

### 1. Register a User

```bash
curl -X POST "https://localhost:5001/api/Usuarios/Registrar" \
  -H "Content-Type: application/json" \
  -d '{
    "correo": "test@example.com",
    "contraseña": "Test123!",
    "nombre": "Test",
    "apellidos": "User",
    "cedula": "123456789"
  }'
```

### 2. Login

```bash
curl -X POST "https://localhost:5001/api/Usuarios/Login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

Save the token from the response.

### 3. Create an Order

```bash
curl -X POST "https://localhost:5001/api/Ordenes/crear" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "servicioId": 1,
    "placaVehiculo": "ABC-123",
    "hora": 9,
    "dia": "2024-12-25"
  }'
```

## Common Commands

### Build the Project

```bash
dotnet build
```

### Restore Dependencies

```bash
dotnet restore
```

### Run Tests (if available)

```bash
dotnet test
```

### Create a Migration

```bash
cd Presentacion
dotnet ef migrations add MigrationName --project ../Infraestructura
```

### Apply Migrations

```bash
dotnet ef database update --project ../Infraestructura
```

## Troubleshooting

### Port Already in Use

Change the port in `Properties/launchSettings.json`:

```json
{
  "applicationUrl": "https://localhost:5002;http://localhost:5003"
}
```

### Database Connection Error

1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure database exists or use `Initial Catalog=master` to create it

### Migration Errors

Reset migrations (development only):

```bash
dotnet ef database drop --project ../Infraestructura
dotnet ef migrations remove --project ../Infraestructura
dotnet ef migrations add InitialCreate --project ../Infraestructura
dotnet ef database update --project ../Infraestructura
```

### SSL Certificate Error

For development, you can disable SSL validation or trust the development certificate:

```bash
dotnet dev-certs https --trust
```

## Next Steps

- Read the [README.md](README.md) for detailed documentation
- Check [API_DOCUMENTATION.md](API_DOCUMENTATION.md) for all endpoints
- Review [CONTRIBUTING.md](CONTRIBUTING.md) if you want to contribute

## Getting Help

- Check existing [Issues](https://github.com/yourusername/rym-lubricentro-api/issues)
- Review the [API Documentation](API_DOCUMENTATION.md)
- Open a new issue for bugs or questions

---

**Happy Coding! 🚀**

