# TaskManager

## Estado actual

Solución en desarrollo con proyectos separados para `Domain`, `Application`, `Infrastructure`, `Identity` y `Web`. `Web` contiene la API de tareas y la base de Razor/Blazor. `Identity` se agregó recientemente como servicio separado de autenticación.

El CRUD de tareas está implementado a nivel de API y persiste en SQL Server. El árbol de trabajo también contiene cambios locales aún no confirmados que trasladan el modelo de usuario desde `Domain` a `Identity`.

## Implementado

- Estructura de solución y referencias entre `Domain`, `Application`, `Infrastructure` y `Web`; el proyecto `Identity` ya está incluido en la solución.
- Dominio de tareas: `TaskItem`, `TaskShare`, `ScheduledTask`, estados, prioridades y permisos de compartición.
- Persistencia de tareas con Entity Framework Core y SQL Server, configuraciones por entidad y migración inicial.
- Repositorio de tareas, `UnitOfWork`, servicio de aplicación y DTOs para crear, actualizar y responder tareas.
- `TasksController` con rutas CRUD bajo `api/tasks`; las consultas del repositorio filtran por `UserId`.
- `ICurrentUser`/`CurrentUser` en Infrastructure para leer la identidad desde claims HTTP.
- En `TaskManager.Identity`: modelo `ApplicationUser`, `IdentityDbContext` con configuración de usuarios e índices únicos para nombre y correo, y servicio de autenticación.
- El servicio de Identity implementa registro e inicio de sesión, hashing/verificación con `IPasswordHasher` y generación de JWT con expiración configurable.
- El host de Identity registra EF Core, autenticación JWT y autorización, y ejecuta `UseAuthentication`/`UseAuthorization`.

## Cambios locales y desalineaciones

- `TaskManager.Identity` y su inclusión en la solución están sin confirmar en Git.
- Se eliminaron localmente `Domain.Entities.User`, `UserConfiguration` y `DbSet<User>`; `TaskItem.UserId` y `TaskShare.UserId` permanecen como GUID sin relación a usuario.
- La migración inicial de `TaskManagerDbContext` todavía crea `Users` y claves foráneas hacia ella. El modelo de Infrastructure y su migración/snapshot deben alinearse antes de aplicar una nueva estrategia de base de datos.
- `IdentityDbContext` apunta a una base `TaskManagerIdentityDb`, pero aún no se observa una migración propia ni automatización de creación del esquema.
- Identity no tiene todavía un `AuthenticationController`; el servicio de registro/login existe, pero no hay rutas HTTP que lo expongan.
- La aplicación `TaskManager.Web` no registra autenticación JWT ni middleware de autenticación/autorización, y no consume el token emitido por Identity. `CurrentUser` solo puede obtener una identidad si el host le entrega claims autenticados.
- `TaskManager.Identity/appsettings.json` contiene una clave JWT de desarrollo. Debe reemplazarse por un secreto seguro provisto fuera del repositorio antes de usar el servicio fuera del entorno local.

## Pendiente / brechas conocidas

- Exponer registro/login mediante endpoints y definir validaciones, respuestas HTTP y manejo de errores para credenciales duplicadas o inválidas.
- Conectar la validación de JWT en `TaskManager.Web` con el mismo issuer, audience y clave/llave configurados en Identity; proteger los endpoints y tomar el `UserId` solo de claims verificados.
- Decidir cómo compartir configuración y despliegue entre los servicios Web e Identity, y cómo aprovisionar sus bases de datos.
- Alinear migraciones, claves foráneas e índices tras el traslado del usuario a Identity; definir cómo migrar una base existente preservando datos.
- Corregir el contrato de errores y validación de la API. `GetById` actualmente responde `200` con `null` cuando no encuentra una tarea; definir respuesta `404`.
- Añadir proyectos y cobertura de pruebas para autenticación, autorización, persistencia y aislamiento de tareas por usuario.
- El CRUD no expone cambio de estado. Compartición funcional y ejecución de tareas programadas tampoco están implementadas.

## Próximos pasos sugeridos

1. Completar el host de Identity con `AuthenticationController` para registro e inicio de sesión; validar entradas y devolver respuestas HTTP consistentes.
2. Crear y aplicar la migración de `IdentityDbContext`; documentar el aprovisionamiento separado de `TaskManagerIdentityDb` y `TaskManagerDb`.
3. Configurar `TaskManager.Web` para validar JWT con parámetros compatibles con Identity y aplicar autorización a los endpoints de tareas.
4. Mover la clave JWT a configuración segura por entorno/secretos y revisar issuer, audience, expiración y rotación antes de despliegue.
5. Alinear el modelo de Infrastructure, el snapshot y las migraciones con usuarios administrados por Identity; decidir la transición para bases ya creadas.
6. Definir validaciones y errores de la API, incluida la respuesta `404` para una tarea inexistente.
7. Añadir pruebas unitarias y de integración para registro/login, emisión y validación de JWT, acceso anónimo y propiedad de tareas.
8. Implementar cambio de estado de tarea y después compartición con aplicación efectiva de permisos.
9. Implementar ejecución de tareas programadas y decidir si requiere proceso de fondo y notificaciones.
10. Priorizar perfil/preferencias, dashboards e internacionalización según necesidades; posponer OData y microservicios adicionales hasta justificar su complejidad.

## Alcance aún no implementado

Endpoints HTTP de autenticación, integración Identity-Web, migración de Identity, configuración segura de claves, alineación de migraciones de tareas, autorización efectiva, validación/errores normalizados, pruebas automatizadas, cambio de estado vía API, compartición funcional, ejecución de tareas programadas, notificaciones, perfil/preferencias, dashboards e internacionalización.
