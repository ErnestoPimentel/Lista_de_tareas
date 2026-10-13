# TaskManager

TaskManager es una solución en desarrollo para administrar tareas. Está construida con .NET 10, ASP.NET Core, Blazor Server, Entity Framework Core y SQL Server. La solución separa el dominio, la lógica de aplicación, la infraestructura, la interfaz/API de tareas y el servicio de identidad.

## Funcionalidad disponible

- Registro e inicio de sesión con contraseña cifrada mediante `IPasswordHasher`.
- Emisión y validación de tokens JWT.
- API autenticada para crear, consultar, actualizar y eliminar tareas propias.
- Tareas con título, descripción, prioridad, estado y fecha de vencimiento.
- Interfaz Blazor Server en el proyecto `TaskManager.Web`.
- Persistencia SQL Server y migraciones de Entity Framework Core.

Compartir tareas, ejecutar tareas programadas, cambiar el estado desde la API y las pruebas automatizadas todavía no están implementados.

## Requisitos

- .NET SDK 10.0.
- SQL Server. La configuración incluida usa LocalDB, disponible con SQL Server Express/Visual Studio en Windows. Para otro entorno, configura las cadenas de conexión con variables de entorno.
- Entity Framework Core CLI (`dotnet-ef`) para aplicar migraciones.

Comprueba el SDK instalado:

```bash
dotnet --version
```

Instala la herramienta EF Core si aún no está disponible:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

Si ya la tienes instalada, puedes actualizarla con `dotnet tool update --global dotnet-ef --version 10.0.12`.

## Configuración local

La aplicación usa dos bases de datos independientes:

| Servicio | Variable de conexión | Base predeterminada |
| --- | --- | --- |
| `TaskManager.Web` | `ConnectionStrings__TaskManager` | `TaskManagerDb` |
| `TaskManager.Identity` | `ConnectionStrings__Identity` | `TaskManagerIdentityDb` |

Ambas cadenas de conexión vienen configuradas para LocalDB en `appsettings.json`. Para SQL Server en otra máquina, define las variables correspondientes antes de iniciar cada proceso. Ejemplo en PowerShell:

```powershell
$env:ConnectionStrings__TaskManager = "Server=localhost;Database=TaskManagerDb;User Id=sa;Password=<contraseña>;TrustServerCertificate=True"
$env:ConnectionStrings__Identity = "Server=localhost;Database=TaskManagerIdentityDb;User Id=sa;Password=<contraseña>;TrustServerCertificate=True"
```

Identity firma los JWT y Web los valida. Ambos deben usar exactamente el mismo secreto, issuer y audience. Configúralos para los dos procesos; en PowerShell:

```powershell
$env:Jwt__Key = "<secreto aleatorio de al menos 32 caracteres>"
$env:Jwt__Issuer = "TaskManager.Identity"
$env:Jwt__Audience = "TaskManager"
```

La configuración del entorno tiene precedencia sobre `appsettings.json`. No uses la clave de ejemplo incluida en Identity fuera del entorno local ni guardes secretos reales en el repositorio.

## Restaurar, compilar y ejecutar

Desde la raíz del repositorio:

```bash
dotnet restore TaskManager.slnx
dotnet build TaskManager.slnx
```

Aplica las migraciones a las dos bases de datos antes del primer inicio:

```bash
dotnet ef database update --project TaskManager.Identity --startup-project TaskManager.Identity --context IdentityDbContext
dotnet ef database update --project TaskManager.Infrastructure --startup-project TaskManager.Web --context TaskManagerDbContext
```

Inicia cada servicio en una terminal distinta:

```bash
dotnet run --project TaskManager.Identity
```

```bash
dotnet run --project TaskManager.Web
```

Los perfiles de desarrollo usan estos puertos:

| Servicio | HTTP | HTTPS |
| --- | --- | --- |
| Identity | `http://localhost:5289` | `https://localhost:7147` |
| Web | `http://localhost:5253` | `https://localhost:7099` |

La interfaz Blazor se sirve desde Web. El documento OpenAPI de Identity está disponible en desarrollo en `http://localhost:5289/openapi/v1.json`.

## API

### Registro

`POST /api/auth/register` en Identity. El nombre debe tener entre 3 y 100 caracteres, el correo debe ser válido y la contraseña debe tener entre 12 y 128 caracteres.

```json
{
  "userName": "ana",
  "email": "ana@example.com",
  "password": "UnaClaveSegura123!"
}
```

Una respuesta correcta incluye `userId`, `userName`, `email`, `accessToken` y `expiresAt`.

### Inicio de sesión

`POST /api/auth/login` en Identity:

```json
{
  "userName": "ana",
  "password": "UnaClaveSegura123!"
}
```

Envía el token devuelto en cada solicitud protegida a Web:

```http
Authorization: Bearer <accessToken>
```

### Tareas

Todas las rutas siguientes pertenecen a `http://localhost:5253/api/tasks` y requieren JWT.

| Método | Ruta | Descripción |
| --- | --- | --- |
| `GET` | `/api/tasks` | Lista las tareas del usuario autenticado. |
| `GET` | `/api/tasks/{taskId}` | Consulta una tarea por identificador. |
| `POST` | `/api/tasks` | Crea una tarea. |
| `PUT` | `/api/tasks/{taskId}` | Actualiza título, descripción, prioridad y vencimiento. |
| `DELETE` | `/api/tasks/{taskId}` | Elimina una tarea. |

Ejemplo de creación:

```json
{
  "title": "Preparar informe",
  "description": "Revisar los avances del proyecto",
  "priority": 1,
  "dueDate": "2026-10-20T18:00:00Z"
}
```

Los valores numéricos de `priority` corresponden al enum `TaskPriority` del dominio. Cada operación de tareas se limita al usuario identificado por el token. Las rutas de autenticación devuelven `409` si el nombre o correo ya están registrados y `401` si las credenciales de acceso son incorrectas.

## Estructura de la solución

| Proyecto | Responsabilidad |
| --- | --- |
| `TaskManager.Domain` | Entidades y enums del dominio. |
| `TaskManager.Application` | Servicios, DTOs e interfaces de aplicación. |
| `TaskManager.Infrastructure` | EF Core, SQL Server y repositorios de tareas. |
| `TaskManager.Identity` | Registro, inicio de sesión, JWT y base de usuarios. |
| `TaskManager.Web` | Interfaz Blazor Server y API de tareas. |

## Migraciones

Las migraciones de usuarios pertenecen a `IdentityDbContext` en `TaskManager.Identity`; las migraciones de tareas pertenecen a `TaskManagerDbContext` en `TaskManager.Infrastructure`. Para crear una migración nueva, especifica el proyecto que contiene el contexto y el proyecto de inicio, por ejemplo:

```bash
dotnet ef migrations add NombreMigracion --project TaskManager.Identity --startup-project TaskManager.Identity --context IdentityDbContext
dotnet ef migrations add NombreMigracion --project TaskManager.Infrastructure --startup-project TaskManager.Web --context TaskManagerDbContext
```

La migración inicial de tareas conserva una tabla `Users` y relaciones heredadas del modelo anterior. Revisa y alinea esa migración antes de usarla para actualizar una base de datos existente o antes de desplegar en producción.

## Estado y limitaciones conocidas

- `TaskManager.Web` necesita `Jwt:Key` para arrancar; define la clave compartida mediante configuración del entorno.
- Las migraciones de tareas contienen el esquema legado de usuarios mencionado arriba.
- `GET /api/tasks/{taskId}` responde actualmente con `200` y `null` cuando la tarea no existe; el contrato puede cambiar a `404`.
- No hay proyectos de pruebas en la solución.
- La compartición, el cambio de estado por API y la ejecución de tareas programadas están pendientes.

Consulta [PROYECTO.md](PROYECTO.md) para conocer el estado de desarrollo y las siguientes tareas previstas.
