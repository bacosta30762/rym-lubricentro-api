# API Documentation

Complete API reference with request/response examples, error handling, and best practices.

## Table of Contents

- [Base Information](#base-information)
- [Authentication](#authentication)
- [User Management](#user-management)
- [Order Management](#order-management)
- [Mechanic Management](#mechanic-management)
- [Article Management](#article-management)
- [Category Management](#category-management)
- [Comments](#comments)
- [Financial Management](#financial-management)
- [Reports](#reports)
- [Marketing](#marketing)
- [Error Handling](#error-handling)
- [Best Practices](#best-practices)

## Base Information

### Base URL

```
Development: http://localhost:5000/api
Production: https://your-api-domain.com/api
```

### Content Type

All requests should use:
```
Content-Type: application/json
```

### Response Format

Successful responses typically return:
- `200 OK` - Success with data
- `201 Created` - Resource created successfully
- `204 No Content` - Success without response body

Error responses:
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

## Authentication

### Register User

Create a new user account.

**Endpoint:** `POST /api/Usuarios/Registrar`

**Authentication:** Not required

**Request Body:**
```json
{
  "correo": "john.doe@example.com",
  "contraseña": "SecurePassword123!",
  "nombre": "John",
  "apellidos": "Doe",
  "cedula": "123456789"
}
```

**Response:** `200 OK`
```json
{
  "mensaje": "Usuario registrado exitosamente."
}
```

**Error Response:** `400 Bad Request`
```json
{
  "errors": [
    "El correo electrónico ya está registrado.",
    "La contraseña debe tener al menos 8 caracteres."
  ]
}
```

**Best Practices:**
- Use strong passwords (min 8 characters, mix of letters, numbers, symbols)
- Validate email format on client side before sending
- Handle errors gracefully and display user-friendly messages

---

### User Login

Authenticate and receive JWT token.

**Endpoint:** `POST /api/Usuarios/Login`

**Authentication:** Not required

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
}
```

**Error Response:** `401 Unauthorized`
```json
{
  "errors": [
    "Credenciales inválidas."
  ]
}
```

**Usage Example:**
```javascript
// Store token after login
const response = await fetch('/api/Usuarios/Login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ email, password })
});
const { token } = await response.json();
localStorage.setItem('authToken', token);

// Use token in subsequent requests
fetch('/api/Ordenes/listar-ordenes-usuario', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
```

---

### Admin Login

Authenticate as administrator.

**Endpoint:** `POST /api/Usuarios/LoginAdmin`

**Authentication:** Not required

**Request Body:**
```json
{
  "email": "admin@lubricentrorym.com",
  "password": "AdminPassword123!"
}
```

**Response:** Same as regular login

**Note:** Admin login validates that the user has Admin role.

---

### Recover Password

Request password reset token via email.

**Endpoint:** `POST /api/Usuarios/RecuperarContraseña`

**Authentication:** Not required

**Request Body:**
```json
{
  "email": "john.doe@example.com"
}
```

**Response:** `200 OK`
```json
{
  "mensaje": "Token de recuperación enviado al correo"
}
```

**Error Response:** `400 Bad Request`
```json
{
  "errors": [
    "Usuario no encontrado."
  ]
}
```

---

### Reset Password

Reset password using recovery token.

**Endpoint:** `POST /api/Usuarios/restablecer-password`

**Authentication:** Not required

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "token": "recovery-token-from-email",
  "nuevaContraseña": "NewSecurePassword123!"
}
```

**Response:** `200 OK`
```json
"La contraseña ha sido restablecida con éxito"
```

---

### Get User Information

Retrieve user details by ID number.

**Endpoint:** `GET /api/Usuarios/ObtenerUsuario/{cedula}`

**Authentication:** Required

**Path Parameters:**
- `cedula` (string) - User ID number

**Response:** `200 OK`
```json
{
  "id": "user-guid",
  "cedula": "123456789",
  "nombre": "John",
  "apellidos": "Doe",
  "email": "john.doe@example.com",
  "activo": true
}
```

---

### Update User

Update user information.

**Endpoint:** `PUT /api/Usuarios/Actualizar/{cedula}`

**Authentication:** Required

**Path Parameters:**
- `cedula` (string) - User ID number

**Request Body:**
```json
{
  "cedula": "123456789",
  "nombre": "John",
  "apellidos": "Doe Updated",
  "email": "john.doe@example.com"
}
```

**Response:** `200 OK`
```json
{
  "fueExitoso": true,
  "mensaje": "Usuario actualizado exitosamente."
}
```

---

### List All Users

Get list of all registered users (Admin only).

**Endpoint:** `GET /api/Usuarios/lista-usuarios`

**Authentication:** Required

**Response:** `200 OK`
```json
[
  {
    "id": "user-guid-1",
    "cedula": "123456789",
    "nombre": "John",
    "apellidos": "Doe",
    "email": "john.doe@example.com",
    "activo": true
  },
  {
    "id": "user-guid-2",
    "cedula": "987654321",
    "nombre": "Jane",
    "apellidos": "Smith",
    "email": "jane.smith@example.com",
    "activo": true
  }
]
```

---

### Assign Role

Assign a role to a user.

**Endpoint:** `POST /api/Usuarios/AsignarRol`

**Authentication:** Required (Admin)

**Request Body:**
```json
{
  "usuarioId": "user-guid",
  "rol": "Mecanico"
}
```

**Available Roles:**
- `Admin` - Full system access
- `Mecanico` - Mechanic access
- `Customer` - Customer access (default)

**Response:** `200 OK`
```json
{
  "fueExitoso": true,
  "mensaje": "Rol asignado exitosamente."
}
```

---

## Order Management

### Create Order

Create a new service order.

**Endpoint:** `POST /api/Ordenes/crear`

**Authentication:** Required

**Request Body:**
```json
{
  "servicioId": 1,
  "placaVehiculo": "ABC-123",
  "hora": 9,
  "dia": "2024-12-25"
}
```

**Field Descriptions:**
- `servicioId` (int) - Service type ID
- `placaVehiculo` (string) - Vehicle license plate
- `hora` (int) - Hour slot (0-23)
- `dia` (DateOnly) - Date in format "YYYY-MM-DD"

**Response:** `200 OK`
```json
"Orden creada con éxito."
```

**Error Response:** `400 Bad Request`
```json
{
  "errors": [
    "La hora seleccionada no está disponible.",
    "El día seleccionado está bloqueado."
  ]
}
```

**Best Practices:**
- Check available hours before creating order
- Validate date is not in the past
- Ensure time slot is available

---

### Get Available Hours

Get available time slots for a service on a specific date.

**Endpoint:** `GET /api/Ordenes/horas-disponibles?servicioId={id}&dia={date}`

**Authentication:** Not required

**Query Parameters:**
- `servicioId` (int) - Service type ID
- `dia` (DateOnly) - Date in format "YYYY-MM-DD"

**Response:** `200 OK`
```json
[9, 10, 11, 14, 15, 16]
```

**Example:**
```javascript
const servicioId = 1;
const dia = "2024-12-25";
const response = await fetch(
  `/api/Ordenes/horas-disponibles?servicioId=${servicioId}&dia=${dia}`
);
const horasDisponibles = await response.json();
// horasDisponibles = [9, 10, 11, 14, 15, 16]
```

---

### List User Orders

Get all orders for the authenticated user.

**Endpoint:** `GET /api/Ordenes/listar-ordenes-usuario`

**Authentication:** Required

**Response:** `200 OK`
```json
[
  {
    "numeroOrden": 1,
    "servicioId": 1,
    "servicioNombre": "Cambio de Aceite",
    "estado": "Pendiente",
    "placaVehiculo": "ABC-123",
    "hora": 9,
    "dia": "2024-12-25",
    "mecanicoAsignado": null
  },
  {
    "numeroOrden": 2,
    "servicioId": 2,
    "servicioNombre": "Alineación",
    "estado": "En Proceso",
    "placaVehiculo": "XYZ-789",
    "hora": 14,
    "dia": "2024-12-26",
    "mecanicoAsignado": "Juan Pérez"
  }
]
```

---

### List All Orders (Admin)

Get all orders in the system.

**Endpoint:** `GET /api/Ordenes/listar-todas-ordenes`

**Authentication:** Required (Admin role)

**Response:** `200 OK`
```json
[
  {
    "numeroOrden": 1,
    "clienteId": "user-guid-1",
    "clienteNombre": "John Doe",
    "servicioId": 1,
    "servicioNombre": "Cambio de Aceite",
    "estado": "Pendiente",
    "placaVehiculo": "ABC-123",
    "hora": 9,
    "dia": "2024-12-25",
    "mecanicoAsignado": null
  }
]
```

---

### List Mechanic Orders

Get orders assigned to the authenticated mechanic.

**Endpoint:** `GET /api/Ordenes/listar-ordenes-asignadas`

**Authentication:** Required (Mechanic role)

**Response:** `200 OK`
```json
[
  {
    "numeroOrden": 1,
    "clienteNombre": "John Doe",
    "servicioNombre": "Cambio de Aceite",
    "estado": "En Proceso",
    "placaVehiculo": "ABC-123",
    "hora": 9,
    "dia": "2024-12-25"
  }
]
```

---

### Assign Mechanic

Assign a mechanic to an order.

**Endpoint:** `PUT /api/Ordenes/asignar-mecanico?numeroOrden={id}&mecanicoId={id}`

**Authentication:** Required (Admin)

**Query Parameters:**
- `numeroOrden` (int) - Order number
- `mecanicoId` (string) - Mechanic user ID

**Response:** `200 OK`
```json
"Mecánico asignado con éxito a la orden."
```

**Error Response:** `400 Bad Request`
```json
{
  "errors": [
    "El mecánico no está disponible en ese horario."
  ]
}
```

---

### Update Order Status

Update the status of an order.

**Endpoint:** `PUT /api/Ordenes/actualizar-estado`

**Authentication:** Required (Admin/Mechanic)

**Request Body:**
```json
{
  "numeroOrden": 1,
  "estado": "Completada"
}
```

**Valid States:**
- `Pendiente` - Order created, awaiting assignment
- `En Proceso` - Order in progress
- `Completada` - Order completed
- `Cancelada` - Order cancelled

**Response:** `200 OK`
```json
{
  "mensaje": "Estado de la orden actualizada correctamente."
}
```

---

### Create Order (Admin)

Create an order on behalf of a customer.

**Endpoint:** `POST /api/Ordenes/crear-orden-admin`

**Authentication:** Required (Admin)

**Request Body:**
```json
{
  "clienteId": "user-guid",
  "servicioId": 1,
  "placaVehiculo": "ABC-123",
  "hora": 9,
  "dia": "2024-12-25"
}
```

**Response:** `200 OK`
```json
{
  "mensaje": "Orden creada exitosamente."
}
```

---

### Block Date

Block a date to prevent order scheduling.

**Endpoint:** `POST /api/Ordenes/bloquear-dia`

**Authentication:** Required (Admin)

**Request Body:**
```json
{
  "dia": "2024-12-31",
  "razon": "Año Nuevo - Cerrado"
}
```

**Response:** `200 OK`
```json
{
  "mensaje": "Día bloqueado exitosamente."
}
```

---

### Unblock Date

Unblock a previously blocked date.

**Endpoint:** `POST /api/Ordenes/desbloquear-dia`

**Authentication:** Required (Admin)

**Request Body:**
```json
{
  "dia": "2024-12-31"
}
```

**Response:** `200 OK`
```json
{
  "mensaje": "Día desbloqueado exitosamente."
}
```

---

### Get Blocked Dates

Get list of all blocked dates.

**Endpoint:** `GET /api/Ordenes/obtener-dias-bloqueados`

**Authentication:** Not required

**Response:** `200 OK`
```json
[
  "2024-12-25",
  "2024-12-31",
  "2025-01-01"
]
```

---

### Delete Order

Delete an order.

**Endpoint:** `DELETE /api/Ordenes/Eliminarorden{id}`

**Authentication:** Required (Admin)

**Path Parameters:**
- `id` (int) - Order number

**Response:** `204 No Content`

**Error Response:** `404 Not Found`
```json
{
  "message": "Orden no encontrada."
}
```

---

## Mechanic Management

### List All Mechanics

Get list of all registered mechanics.

**Endpoint:** `GET /api/Mecanicos/obtener-mecanicos`

**Authentication:** Not required

**Response:** `200 OK`
```json
[
  {
    "usuarioId": "mechanic-guid-1",
    "nombre": "Juan",
    "apellidos": "Pérez",
    "email": "juan.perez@lubricentrorym.com",
    "servicios": [
      {
        "id": 1,
        "nombre": "Cambio de Aceite"
      },
      {
        "id": 2,
        "nombre": "Alineación"
      }
    ]
  }
]
```

---

### Assign Services to Mechanic

Assign services that a mechanic can perform.

**Endpoint:** `POST /api/Mecanicos/asignar-servicios/{usuarioId}`

**Authentication:** Required (Admin)

**Path Parameters:**
- `usuarioId` (string) - Mechanic user ID

**Request Body:**
```json
[1, 2, 3]
```

**Response:** `200 OK`
```json
"Servicios asignados con éxito."
```

**Error Response:** `400 Bad Request`
```json
{
  "errors": [
    "El usuarioId es requerido.",
    "Debe proporcionar al menos un servicioId."
  ]
}
```

---

## Article Management

### List All Articles

Get all available articles/products.

**Endpoint:** `GET /api/Articulo/Listar`

**Authentication:** Not required

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "nombre": "Aceite Motor 5W-30",
    "descripcion": "Aceite sintético de alta calidad",
    "precio": 25.99,
    "stock": 50,
    "categoriaId": 1,
    "categoriaNombre": "Aceites"
  },
  {
    "id": 2,
    "nombre": "Filtro de Aire",
    "descripcion": "Filtro de aire premium",
    "precio": 15.50,
    "stock": 30,
    "categoriaId": 2,
    "categoriaNombre": "Filtros"
  }
]
```

---

### Get Article by ID

Get specific article details.

**Endpoint:** `GET /api/Articulo/Obtener{id}`

**Authentication:** Not required

**Path Parameters:**
- `id` (int) - Article ID

**Response:** `200 OK`
```json
{
  "id": 1,
  "nombre": "Aceite Motor 5W-30",
  "descripcion": "Aceite sintético de alta calidad",
  "precio": 25.99,
  "stock": 50,
  "categoriaId": 1,
  "categoriaNombre": "Aceites"
}
```

**Error Response:** `404 Not Found`

---

### List Articles by Category

Get articles filtered by category.

**Endpoint:** `GET /api/Articulo/ListarPorCategoria/{categoriaId}`

**Authentication:** Not required

**Path Parameters:**
- `categoriaId` (int) - Category ID

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "nombre": "Aceite Motor 5W-30",
    "precio": 25.99,
    "stock": 50
  }
]
```

---

### Create Article

Create a new article/product.

**Endpoint:** `POST /api/Articulo/Agregar`

**Authentication:** Required

**Request Body:**
```json
{
  "nombre": "Aceite Motor 5W-30",
  "descripcion": "Aceite sintético de alta calidad",
  "precio": 25.99,
  "stock": 50,
  "categoriaId": 1
}
```

**Response:** `201 Created`
```json
{
  "id": 1,
  "nombre": "Aceite Motor 5W-30",
  "descripcion": "Aceite sintético de alta calidad",
  "precio": 25.99,
  "stock": 50,
  "categoriaId": 1
}
```

---

### Update Article

Update an existing article.

**Endpoint:** `PUT /api/Articulo/Actualizar{id}`

**Authentication:** Required

**Path Parameters:**
- `id` (int) - Article ID

**Request Body:**
```json
{
  "id": 1,
  "nombre": "Aceite Motor 5W-30 Premium",
  "descripcion": "Aceite sintético de alta calidad - Actualizado",
  "precio": 29.99,
  "stock": 45,
  "categoriaId": 1
}
```

**Response:** `204 No Content`

**Error Response:** `400 Bad Request`
```json
"El ID no coincide."
```

---

### Delete Article

Delete an article.

**Endpoint:** `DELETE /api/Articulo/Eliminar{id}`

**Authentication:** Required

**Path Parameters:**
- `id` (int) - Article ID

**Response:** `204 No Content`

---

## Category Management

### List All Categories

Get all product categories.

**Endpoint:** `GET /api/Categoria/Listar`

**Authentication:** Not required

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "nombre": "Aceites",
    "descripcion": "Aceites para motor"
  },
  {
    "id": 2,
    "nombre": "Filtros",
    "descripcion": "Filtros de aire, aceite, etc."
  }
]
```

---

### Get Category by ID

Get specific category details.

**Endpoint:** `GET /api/Categoria/Obtener/{id}`

**Authentication:** Not required

**Path Parameters:**
- `id` (int) - Category ID

**Response:** `200 OK`
```json
{
  "id": 1,
  "nombre": "Aceites",
  "descripcion": "Aceites para motor"
}
```

---

## Comments

### Create Comment

Add a comment to an order.

**Endpoint:** `POST /api/Comentarios/crear`

**Authentication:** Required

**Request Body:**
```json
{
  "ordenId": 1,
  "contenido": "Excelente servicio, muy profesional."
}
```

**Response:** `200 OK`
```json
{
  "id": 1
}
```

---

### Edit Comment

Update an existing comment.

**Endpoint:** `PUT /api/Comentarios/editar/{id}`

**Authentication:** Required

**Path Parameters:**
- `id` (int) - Comment ID

**Request Body:**
```json
{
  "ordenId": 1,
  "contenido": "Servicio actualizado - Excelente atención."
}
```

**Response:** `200 OK`

---

### Delete Comment

Delete a comment.

**Endpoint:** `DELETE /api/Comentarios/eliminar/{id}`

**Authentication:** Required

**Path Parameters:**
- `id` (int) - Comment ID

**Response:** `200 OK`

---

### Get User Comments

Get all comments by a specific user.

**Endpoint:** `GET /api/Comentarios/obtenercomentariosusuario/{usuarioId}`

**Authentication:** Not required

**Path Parameters:**
- `usuarioId` (string) - User ID

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "ordenId": 1,
    "contenido": "Excelente servicio",
    "fechaCreacion": "2024-12-20T10:30:00Z",
    "respuesta": null
  }
]
```

---

### Get All Comments

Get all comments in the system.

**Endpoint:** `GET /api/Comentarios/obtenercomentarios`

**Authentication:** Not required

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "usuarioNombre": "John Doe",
    "ordenId": 1,
    "contenido": "Excelente servicio",
    "fechaCreacion": "2024-12-20T10:30:00Z",
    "respuesta": "Gracias por su comentario"
  }
]
```

---

### Reply to Comment

Add a reply to a comment.

**Endpoint:** `POST /api/Comentarios/responder/{id}`

**Authentication:** Required

**Path Parameters:**
- `id` (int) - Comment ID

**Request Body:**
```json
{
  "respuesta": "Gracias por su comentario. Nos alegra saber que quedó satisfecho."
}
```

**Response:** `200 OK`

---

## Financial Management

### Income Management

#### List All Income Records

**Endpoint:** `GET /api/Ingreso/Listar`

**Authentication:** Required

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "descripcion": "Venta de servicios",
    "monto": 150.00,
    "fecha": "2024-12-20",
    "categoria": "Servicios"
  }
]
```

#### Create Income Record

**Endpoint:** `POST /api/Ingreso/Crear`

**Authentication:** Required

**Request Body:**
```json
{
  "descripcion": "Venta de servicios",
  "monto": 150.00,
  "fecha": "2024-12-20",
  "categoria": "Servicios"
}
```

**Response:** `201 Created`

#### Update Income Record

**Endpoint:** `PUT /api/Ingreso/Actualizar/{id}`

**Authentication:** Required

#### Delete Income Record

**Endpoint:** `DELETE /api/Ingreso/Eliminar/{id}`

**Authentication:** Required

---

### Expense Management

#### List All Expense Records

**Endpoint:** `GET /api/Egreso/Listar`

**Authentication:** Required

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "descripcion": "Compra de repuestos",
    "monto": 75.50,
    "fecha": "2024-12-20",
    "categoria": "Inventario"
  }
]
```

#### Create Expense Record

**Endpoint:** `POST /api/Egreso/Crear`

**Authentication:** Required

**Request Body:**
```json
{
  "descripcion": "Compra de repuestos",
  "monto": 75.50,
  "fecha": "2024-12-20",
  "categoria": "Inventario"
}
```

**Response:** `201 Created`

---

## Reports

### Financial Report

Get financial report for a date range.

**Endpoint:** `GET /api/ReporteFinanciero?fechaInicio={date}&fechaFin={date}`

**Authentication:** Required

**Query Parameters:**
- `fechaInicio` (DateTime) - Start date
- `fechaFin` (DateTime) - End date

**Example:**
```
GET /api/ReporteFinanciero?fechaInicio=2024-12-01&fechaFin=2024-12-31
```

**Response:** `200 OK`
```json
{
  "fechaInicio": "2024-12-01",
  "fechaFin": "2024-12-31",
  "totalIngresos": 5000.00,
  "totalEgresos": 2500.00,
  "gananciaNeta": 2500.00,
  "ingresos": [
    {
      "id": 1,
      "descripcion": "Venta de servicios",
      "monto": 150.00,
      "fecha": "2024-12-20"
    }
  ],
  "egresos": [
    {
      "id": 1,
      "descripcion": "Compra de repuestos",
      "monto": 75.50,
      "fecha": "2024-12-20"
    }
  ]
}
```

**Error Response:** `400 Bad Request`
```json
"La fecha de inicio no puede ser mayor a la fecha de fin."
```

---

## Marketing

### Newsletter Management

#### Create Newsletter

**Endpoint:** `POST /api/Marketing/CrearBoletin`

**Authentication:** Required (Admin)

**Request Body:**
```json
{
  "titulo": "Ofertas de Diciembre",
  "contenido": "Aprovecha nuestras ofertas especiales...",
  "fechaPublicacion": "2024-12-20"
}
```

#### List Newsletters

**Endpoint:** `GET /api/Marketing/ObtenerBoletines`

**Authentication:** Not required

#### Update Newsletter

**Endpoint:** `PUT /api/Marketing/ModificarBoletin`

**Authentication:** Required (Admin)

#### Delete Newsletter

**Endpoint:** `DELETE /api/Marketing/EliminarBoletin/{id}`

**Authentication:** Required (Admin)

---

### Reviews Management

#### Create Review

**Endpoint:** `POST /api/Marketing/CrearResena`

**Authentication:** Not required

**Request Body:**
```json
{
  "nombreCliente": "John Doe",
  "calificacion": 5,
  "comentario": "Excelente servicio, muy recomendado.",
  "fecha": "2024-12-20"
}
```

#### List Reviews

**Endpoint:** `GET /api/Marketing/ObtenerResenas`

**Authentication:** Not required

---

### Subscriptions

#### Create Subscription

**Endpoint:** `POST /api/Marketing/CrearSuscripcion`

**Authentication:** Not required

**Request Body:**
```json
{
  "email": "customer@example.com",
  "activo": true
}
```

#### List Subscriptions

**Endpoint:** `GET /api/Marketing/ObtenerSuscripciones`

**Authentication:** Required (Admin)

---

## Error Handling

### Standard Error Response Format

```json
{
  "errors": [
    "Error message 1",
    "Error message 2"
  ]
}
```

### HTTP Status Codes

- **200 OK** - Request successful
- **201 Created** - Resource created successfully
- **204 No Content** - Success without response body
- **400 Bad Request** - Invalid request data
- **401 Unauthorized** - Authentication required
- **403 Forbidden** - Insufficient permissions
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

### Common Error Scenarios

#### Invalid Token
```json
{
  "error": "Invalid token"
}
```

#### Validation Errors
```json
{
  "errors": [
    "El campo 'email' es requerido.",
    "La contraseña debe tener al menos 8 caracteres."
  ]
}
```

#### Resource Not Found
```json
{
  "message": "Usuario no encontrado"
}
```

---

## Best Practices

### 1. Authentication

- Always store JWT tokens securely (localStorage, sessionStorage, or httpOnly cookies)
- Include token in Authorization header: `Bearer {token}`
- Handle token expiration gracefully
- Implement token refresh mechanism if available

### 2. Error Handling

- Always check response status before processing data
- Display user-friendly error messages
- Log errors for debugging
- Implement retry logic for network failures

### 3. Request Optimization

- Use appropriate HTTP methods (GET for read, POST for create, PUT for update, DELETE for delete)
- Implement pagination for large datasets
- Cache frequently accessed data
- Use query parameters for filtering

### 4. Data Validation

- Validate data on client side before sending
- Handle server-side validation errors
- Provide clear validation messages
- Sanitize user input

### 5. Security

- Never expose sensitive data in URLs
- Use HTTPS in production
- Implement rate limiting
- Validate and sanitize all inputs
- Use parameterized queries (handled by EF Core)

### 6. Date Handling

- Use ISO 8601 format (YYYY-MM-DD) for dates
- Handle timezone conversions appropriately
- Validate date ranges before sending requests

### 7. Code Examples

#### JavaScript/TypeScript Fetch Example

```typescript
class ApiClient {
  private baseUrl = 'https://api.example.com/api';
  private token: string | null = null;

  setToken(token: string) {
    this.token = token;
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
      ...options.headers,
    };

    if (this.token) {
      headers['Authorization'] = `Bearer ${this.token}`;
    }

    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      ...options,
      headers,
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.errors?.join(', ') || 'Request failed');
    }

    return response.json();
  }

  async login(email: string, password: string) {
    const response = await this.request<{ token: string }>(
      '/Usuarios/Login',
      {
        method: 'POST',
        body: JSON.stringify({ email, password }),
      }
    );
    this.setToken(response.token);
    return response;
  }

  async getOrders() {
    return this.request<ListarOrdenDto[]>('/Ordenes/listar-ordenes-usuario');
  }
}
```

#### C# HttpClient Example

```csharp
public class ApiClient
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.example.com/api/");
    }

    public void SetToken(string token)
    {
        _token = token;
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<RespuestaLoginDto> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("Usuarios/Login", loginDto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RespuestaLoginDto>();
        SetToken(result.Token);
        return result;
    }

    public async Task<List<ListarOrdenDto>> GetOrdersAsync()
    {
        var response = await _httpClient.GetAsync("Ordenes/listar-ordenes-usuario");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ListarOrdenDto>>();
    }
}
```

---

## Rate Limiting

Currently, the API does not implement rate limiting. For production use, consider implementing:

- Per-user rate limits
- Per-IP rate limits
- Endpoint-specific limits

## Support

For API support or questions:
- Open an issue in the repository
- Email: support@lubricentrorym.com
- Check Swagger documentation at `/swagger` when running locally

---

**Last Updated:** December 2024
**API Version:** 1.0

