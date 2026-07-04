# Solución Gestión de Trabajo - Microservicios .NET 8

## 1. Descripción general

Este proyecto corresponde a una prueba práctica backend desarrollada en **C# con .NET 8**, utilizando **SQL Server** y una arquitectura basada en **microservicios**.

La solución está dividida en dos proyectos independientes:

```text
SolucionGestionTrabajo
│
├── GestionUsuarios.Api
└── ItemsTrabajo.Api
```

Cada proyecto representa un microservicio con responsabilidades separadas.

---

## 2. Arquitectura utilizada

La solución aplica una arquitectura simple por capas:

```text
Controller → Servicio → Repositorio → Base de Datos
```

### Controller

Recibe las peticiones HTTP desde Swagger o desde cualquier cliente externo.

### Servicio

Contiene la lógica de negocio del aplicativo.

En el microservicio de ítems de trabajo, esta capa se encarga de aplicar las reglas de distribución.

### Repositorio

Se encarga del acceso a datos usando Entity Framework Core con enfoque **Database First**.

### Models

Contiene las entidades generadas desde SQL Server mediante Scaffold-DbContext.

### DTOs

Contiene los objetos usados para recibir o devolver información sin exponer directamente detalles internos innecesarios.

---

## 3. Microservicio GestionUsuarios.Api

Este microservicio administra los usuarios existentes en el sistema.

### Responsabilidades

- Listar usuarios activos.
- Consultar usuario por identificador.
- Crear usuarios.
- Servir como fuente de usuarios para el microservicio de ítems de trabajo.

### Base de datos

```text
BD_GestionUsuarios
```

### Tabla principal

```text
Usuarios
```

### Endpoints principales

```http
GET /api/Usuarios
GET /api/Usuarios/{idUsuario}
POST /api/Usuarios
```

### Ejemplo para crear usuario

```json
{
  "nombreCompleto": "Usuario C",
  "correo": "usuarioc@test.com"
}
```

---

## 4. Microservicio ItemsTrabajo.Api

Este microservicio administra los ítems de trabajo y realiza la asignación automática a usuarios.

### Responsabilidades

- Crear ítems de trabajo.
- Listar ítems de trabajo.
- Consultar un ítem por identificador.
- Marcar ítems como completados.
- Consultar pendientes agrupados por usuario.
- Asignar automáticamente ítems según reglas de negocio.

### Base de datos

```text
BD_ItemsTrabajo
```

### Tabla principal

```text
ItemsTrabajo
```

### Endpoints principales

```http
GET /api/ItemsTrabajo
GET /api/ItemsTrabajo/{idItemTrabajo}
POST /api/ItemsTrabajo
PUT /api/ItemsTrabajo/{idItemTrabajo}/completar
GET /api/ItemsTrabajo/pendientes-por-usuario
```

---

## 5. Reglas de distribución implementadas

Cuando se crea un nuevo ítem de trabajo, el sistema consulta los usuarios activos desde el microservicio `GestionUsuarios.Api` y aplica las siguientes reglas:

1. Si la fecha de entrega está próxima a vencer, es decir, en menos de 3 días desde la fecha actual, se asigna al usuario con menos ítems pendientes.
2. Si el ítem es relevante, se asigna al usuario con menor lista de pendientes.
3. Si un usuario tiene más de 3 ítems relevantes pendientes, se considera saturado y no se toma en cuenta para la asignación.
4. Después de cada asignación, se puede consultar la lista de pendientes ordenada por usuario.

---

## 6. Comunicación entre microservicios

El microservicio `ItemsTrabajo.Api` se comunica con `GestionUsuarios.Api` mediante HTTP usando `HttpClient`.

Flujo general:

```text
Cliente / Swagger
    ↓
ItemsTrabajo.Api
    ↓ HTTP
GestionUsuarios.Api
    ↓
Usuarios activos
    ↓
ItemsTrabajo.Api aplica algoritmo de asignación
    ↓
Guarda el ítem asignado en BD_ItemsTrabajo
```

---

## 7. Configuración de bases de datos

La conexión a SQL Server se realiza usando autenticación integrada de Windows.

### GestionUsuarios.Api - appsettings.json

```json
{
  "ConnectionStrings": {
    "ConexionGestionUsuarios": "Server=.;Database=BD_GestionUsuarios;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### ItemsTrabajo.Api - appsettings.json

```json
{
  "ConnectionStrings": {
    "ConexionItemsTrabajo": "Server=.;Database=BD_ItemsTrabajo;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "ServiciosExternos": {
    "GestionUsuariosUrl": "https://localhost:PUERTO_USUARIOS/"
  }
}
```

El valor `PUERTO_USUARIOS` debe reemplazarse por el puerto real donde se ejecuta `GestionUsuarios.Api`.

---

## 8. Ejecución de los microservicios

Para probar correctamente la solución, ambos proyectos deben ejecutarse al mismo tiempo.

### En Visual Studio 2022

1. Abrir la solución `SolucionGestionTrabajo`.
2. Clic derecho sobre la solución.
3. Seleccionar **Configurar proyectos de inicio**.
4. Elegir **Varios proyectos de inicio**.
5. Marcar como `Iniciar`:

```text
GestionUsuarios.Api
ItemsTrabajo.Api
```

6. Ejecutar la solución.

---

## 9. Puertos

Los puertos pueden variar según la configuración local de Visual Studio.

Ejemplo:

```text
GestionUsuarios.Api → https://localhost:7001
ItemsTrabajo.Api    → https://localhost:7002
```

Es importante que el puerto configurado en `ItemsTrabajo.Api` dentro de `ServiciosExternos:GestionUsuariosUrl` coincida con el puerto real de `GestionUsuarios.Api`.

---

## 10. Pruebas con Swagger

Al ejecutar ambos proyectos, Swagger se abre automáticamente para cada microservicio.

### Probar GestionUsuarios.Api

#### Obtener usuarios

```http
GET /api/Usuarios
```

#### Crear usuario

```http
POST /api/Usuarios
```

Body:

```json
{
  "nombreCompleto": "Usuario C",
  "correo": "usuarioc@test.com"
}
```

---

### Probar ItemsTrabajo.Api

#### Crear ítem urgente

```http
POST /api/ItemsTrabajo
```

Body:

```json
{
  "titulo": "Ítem urgente",
  "descripcion": "Debe asignarse al usuario con menos pendientes",
  "esRelevante": false,
  "fechaEntrega": "2026-07-06T00:00:00"
}
```

#### Crear ítem relevante

```http
POST /api/ItemsTrabajo
```

Body:

```json
{
  "titulo": "Ítem relevante",
  "descripcion": "Debe asignarse considerando la menor lista de pendientes",
  "esRelevante": true,
  "fechaEntrega": "2026-07-15T00:00:00"
}
```

#### Marcar ítem como completado

```http
PUT /api/ItemsTrabajo/8/completar
```

#### Consultar pendientes por usuario

```http
GET /api/ItemsTrabajo/pendientes-por-usuario
```

---

## 11. Scripts principales de base de datos

### Crear bases de datos

```sql
CREATE DATABASE BD_GestionUsuarios;
GO

CREATE DATABASE BD_ItemsTrabajo;
GO
```

### Tabla Usuarios

```sql
USE BD_GestionUsuarios;
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(150) NOT NULL,
    Correo NVARCHAR(150) NOT NULL,
    EstaActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO
```

### Tabla ItemsTrabajo

```sql
USE BD_ItemsTrabajo;
GO

CREATE TABLE ItemsTrabajo (
    IdItemTrabajo INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    EsRelevante BIT NOT NULL DEFAULT 0,
    FechaEntrega DATETIME NOT NULL,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    IdUsuarioAsignado INT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO
```

---

## 12. Tecnologías utilizadas

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Database First
- Swagger
- Visual Studio 2022