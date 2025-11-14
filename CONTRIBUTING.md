# Contributing to RyM Lubricentro API

Thank you for your interest in contributing to the RyM Lubricentro API project! This document provides guidelines and instructions for contributing.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)
- [Project Structure](#project-structure)

## Code of Conduct

### Our Pledge

We are committed to providing a welcoming and inclusive environment for all contributors. Please be respectful and professional in all interactions.

### Expected Behavior

- Use welcoming and inclusive language
- Be respectful of differing viewpoints and experiences
- Gracefully accept constructive criticism
- Focus on what is best for the community
- Show empathy towards other community members

## Getting Started

### Prerequisites

Before you begin, ensure you have:

1. Forked the repository
2. Cloned your fork locally
3. Set up the development environment (see [README.md](README.md))
4. Created a branch for your changes

### Setting Up Development Environment

```bash
# Clone your fork
git clone https://github.com/bacosta30762/rym-lubricentro-api.git
cd rym-lubricentro-api

# Add upstream remote
git remote add upstream https://github.com/bacosta30762/rym-lubricentro-api.git

# Install dependencies
dotnet restore

# Run database migrations
cd Presentacion
dotnet ef database update --project ../Infraestructura
```

## Development Workflow

### 1. Create a Feature Branch

Always create a new branch for your work:

```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/bug-description
```

**Branch Naming Conventions:**
- `feature/` - New features
- `fix/` - Bug fixes
- `refactor/` - Code refactoring
- `docs/` - Documentation updates
- `test/` - Test additions or updates

### 2. Make Your Changes

- Write clean, maintainable code
- Follow the coding standards below
- Add tests for new functionality
- Update documentation as needed

### 3. Test Your Changes

```bash
# Build the solution
dotnet build

# Run the application
cd Presentacion
dotnet run

# Test your changes manually or with automated tests
```

### 4. Commit Your Changes

Follow the commit message guidelines below.

### 5. Push and Create Pull Request

```bash
git push origin feature/your-feature-name
```

Then create a Pull Request on GitHub.

## Coding Standards

### C# Coding Conventions

Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions):

#### Naming Conventions

- **Classes**: PascalCase (`UsuarioService`, `OrdenController`)
- **Methods**: PascalCase (`ObtenerUsuarioAsync`, `CrearOrden`)
- **Properties**: PascalCase (`Nombre`, `Email`, `FechaCreacion`)
- **Private fields**: camelCase with underscore prefix (`_usuariosService`, `_dbContext`)
- **Local variables**: camelCase (`usuarioId`, `ordenDto`)
- **Constants**: PascalCase (`MAX_RETRY_COUNT`, `DEFAULT_TIMEOUT`)

#### Code Organization

```csharp
// 1. Using statements
using System;
using Microsoft.AspNetCore.Mvc;

// 2. Namespace
namespace Aplicacion.Servicios
{
    // 3. Class declaration
    public class UsuarioService : IUsuariosService
    {
        // 4. Private fields
        private readonly IUsuarioRepository _repository;
        
        // 5. Constructor
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        
        // 6. Public methods
        public async Task<Resultado> AgregarUsuarioAsync(AgregarUsuarioDto dto)
        {
            // Implementation
        }
        
        // 7. Private methods
        private bool ValidarEmail(string email)
        {
            // Implementation
        }
    }
}
```

#### Async/Await Patterns

Always use async/await for I/O operations:

```csharp
// ✅ Good
public async Task<Usuario> ObtenerUsuarioAsync(string id)
{
    return await _repository.ObtenerPorIdAsync(id);
}

// ❌ Bad
public Usuario ObtenerUsuario(string id)
{
    return _repository.ObtenerPorId(id);
}
```

#### Error Handling

Use the `Resultado` pattern for operations that can fail:

```csharp
public async Task<Resultado> AgregarUsuarioAsync(AgregarUsuarioDto dto)
{
    try
    {
        // Validation
        if (string.IsNullOrEmpty(dto.Email))
        {
            return Resultado.Fallido("El email es requerido.");
        }
        
        // Business logic
        var usuario = new Usuario { Email = dto.Email };
        await _repository.AgregarAsync(usuario);
        
        return Resultado.Exitoso();
    }
    catch (Exception ex)
    {
        // Log error
        return Resultado.Fallido($"Error al agregar usuario: {ex.Message}");
    }
}
```

#### Dependency Injection

Use constructor injection:

```csharp
// ✅ Good
public class OrdenController : ControllerBase
{
    private readonly IOrdenService _ordenService;
    
    public OrdenController(IOrdenService ordenService)
    {
        _ordenService = ordenService;
    }
}

// ❌ Bad
public class OrdenController : ControllerBase
{
    private readonly IOrdenService _ordenService = new OrdenService();
}
```

### Architecture Guidelines

#### Layer Responsibilities

1. **Presentation Layer** (`Presentacion/`)
   - Controllers only handle HTTP concerns
   - No business logic
   - Input validation and response formatting

2. **Application Layer** (`Aplicacion/`)
   - Business logic orchestration
   - DTOs and mapping
   - Validation rules
   - Service interfaces

3. **Domain Layer** (`Dominio/`)
   - Core business entities
   - Domain interfaces
   - Business rules
   - No dependencies on other layers

4. **Infrastructure Layer** (`Infraestructura/`)
   - Data access implementations
   - External service integrations
   - Email services
   - Database context

#### DTOs (Data Transfer Objects)

- Use records for immutable DTOs
- Keep DTOs simple and focused
- Use descriptive names

```csharp
// ✅ Good
public record CrearOrdenDto
(
    int ServicioId,
    string PlacaVehiculo,
    int Hora,
    DateOnly Dia
);

// ❌ Bad
public class OrderDto
{
    public int SId { get; set; }
    public string Plate { get; set; }
    // Unclear abbreviations
}
```

#### Repository Pattern

- One repository per aggregate root
- Repository interfaces in Domain layer
- Implementations in Infrastructure layer

```csharp
// Domain/Repositorios/IUsuarioRepository.cs
public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorIdAsync(string id);
    Task AgregarAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
}
```

### Code Comments

#### XML Documentation

Add XML documentation for public APIs:

```csharp
/// <summary>
/// Creates a new service order for the authenticated user.
/// </summary>
/// <param name="dto">Order creation data</param>
/// <returns>Result indicating success or failure with error messages</returns>
public async Task<Resultado> CrearOrdenAsync(CrearOrdenDto dto)
{
    // Implementation
}
```

#### Inline Comments

Use comments to explain "why", not "what":

```csharp
// ✅ Good - Explains why
// Block date to prevent scheduling during maintenance
await _ordenService.BloquearDiaAsync(dto);

// ❌ Bad - States the obvious
// Call the BloquearDiaAsync method
await _ordenService.BloquearDiaAsync(dto);
```

## Commit Guidelines

### Commit Message Format

Use clear, descriptive commit messages:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

### Examples

```
feat(ordenes): Add order status update endpoint

- Add PUT endpoint for updating order status
- Implement validation for status transitions
- Add unit tests for status update logic

Closes #123
```

```
fix(usuarios): Fix password validation error message

The error message was not displaying correctly when password
validation failed. Updated to show proper validation message.

Fixes #456
```

```
docs(api): Update API documentation with new endpoints

- Add documentation for marketing endpoints
- Include request/response examples
- Add authentication requirements
```

## Pull Request Process

### Before Submitting

1. ✅ Code follows style guidelines
2. ✅ All tests pass
3. ✅ Documentation is updated
4. ✅ No merge conflicts with main branch
5. ✅ Code is reviewed by yourself

### PR Description Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Manual testing completed
- [ ] Unit tests added/updated
- [ ] Integration tests pass

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex code
- [ ] Documentation updated
- [ ] No new warnings generated
- [ ] Tests added/updated
- [ ] All tests pass
```

### Review Process

1. Maintainers will review your PR
2. Address any feedback or requested changes
3. Once approved, your PR will be merged
4. Thank you for contributing! 🎉

## Testing Guidelines

### Unit Testing

Write unit tests for business logic:

```csharp
[Fact]
public async Task CrearOrdenAsync_ValidDto_ReturnsSuccess()
{
    // Arrange
    var dto = new CrearOrdenDto(1, "ABC-123", 9, DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
    var mockRepository = new Mock<IOrdenRepository>();
    var service = new OrdenService(mockRepository.Object);
    
    // Act
    var result = await service.CrearOrdenAsync(dto);
    
    // Assert
    Assert.True(result.FueExitoso);
    mockRepository.Verify(r => r.AgregarAsync(It.IsAny<Orden>()), Times.Once);
}
```

### Integration Testing

Test API endpoints:

```csharp
[Fact]
public async Task CrearOrden_ValidRequest_ReturnsOk()
{
    // Arrange
    var client = _factory.CreateClient();
    var token = await GetAuthTokenAsync(client);
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", token);
    
    var dto = new CrearOrdenDto(1, "ABC-123", 9, DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
    
    // Act
    var response = await client.PostAsJsonAsync("/api/Ordenes/crear", dto);
    
    // Assert
    response.EnsureSuccessStatusCode();
}
```

### Manual Testing

- Test all endpoints with Swagger UI
- Test error scenarios
- Test authentication/authorization
- Test edge cases

## Documentation

### Code Documentation

- Add XML comments to public APIs
- Document complex algorithms
- Explain business rules

### API Documentation

- Update `API_DOCUMENTATION.md` for new endpoints
- Include request/response examples
- Document authentication requirements
- Add error response examples

### README Updates

- Update setup instructions if needed
- Add new features to feature list
- Update dependencies if changed

## Project Structure

### Adding a New Feature

1. **Domain Layer**
   - Create entity if needed (`Dominio/Entidades/`)
   - Create repository interface (`Dominio/Repositorios/`)

2. **Infrastructure Layer**
   - Implement repository (`Infraestructura/`)
   - Add database migration if needed

3. **Application Layer**
   - Create DTOs (`Aplicacion/Usuarios/Dtos/`)
   - Create service interface (`Aplicacion/Interfaces/`)
   - Implement service (`Aplicacion/Servicios/`)
   - Add validators if needed (`Aplicacion/Usuarios/Validadores/`)

4. **Presentation Layer**
   - Create controller (`Presentacion/Controllers/`)
   - Add endpoint documentation

### File Organization

```
FeatureName/
├── Dtos/
│   ├── CrearFeatureDto.cs
│   └── FeatureDto.cs
├── Validadores/
│   └── CrearFeatureValidator.cs
└── Mapeadores/
    └── FeatureProfile.cs (AutoMapper)
```

## Common Issues and Solutions

### Database Migration Conflicts

```bash
# Reset migrations (development only)
dotnet ef database drop --project Infraestructura --startup-project Presentacion
dotnet ef migrations remove --project Infraestructura --startup-project Presentacion
dotnet ef migrations add InitialCreate --project Infraestructura --startup-project Presentacion
dotnet ef database update --project Infraestructura --startup-project Presentacion
```

### Dependency Injection Issues

Ensure services are registered in:
- `Aplicacion/Extensiones/InyeccionDependencias.cs`
- `Infraestructura/Extensiones/InyeccionDependencias.cs`

### Authentication Issues

- Verify JWT settings in `appsettings.json`
- Check token expiration
- Ensure Authorization header format: `Bearer {token}`

## Getting Help

- Open an issue for bugs or feature requests
- Ask questions in discussions
- Review existing issues and PRs
- Check documentation first

## Recognition

Contributors will be recognized in:
- README.md contributors section
- Release notes
- Project documentation

Thank you for contributing to RyM Lubricentro API! 🚀

