# TaskManager

## Estado actual

Solución en desarrollo con proyectos separados para `Domain`, `Application`, `Infrastructure`, `Identity` y `Web`. `Identity` administra cuentas y emite tokens; `Web` expone la API de tareas y la aplicación Razor/Blazor.

El flujo HTTP básico de autenticación está implementado: registro e inicio de sesión emiten JWT, y la API de tareas configura bearer JWT y protege sus endpoints con `[Authorize]`. El último commit integró estos cambios y refactorizó el manejo del usuario; el árbol de trabajo está limpio.

## Implementado

- Dominio de tareas (`TaskItem`, `TaskShare`, `ScheduledTask`), estados, prioridades y permisos de compartición.
- Persistencia de tareas con EF Core y SQL Server, configuraciones, repositorio, `UnitOfWork` y migración inicial.
- Servicio de aplicación y DTOs para crear, consultar, actualizar y eliminar tareas.
- `TasksController` con rutas CRUD bajo `api/tasks`; usa el `UserId` autenticado y filtra las operaciones por propietario.
- `ICurrentUser`/`CurrentUser` para leer el identificador desde el claim `NameIdentifier`.
- Proyecto `Identity` con `ApplicationUser`, `IdentityDbContext`, índices únicos para nombre y correo, y migración inicial para la tabla `Users`.
- Endpoints `POST /api/auth/register` y `POST /api/auth/login`. Registro valida campos, normaliza nombre/correo y evita duplicados; las contraseñas se procesan con `IPasswordHasher`.
- Emisión de JWT con claims de identificador, nombre y correo, y configuración de issuer, audience, expiración y validación de firma.
- Middleware JWT y autorización configurados tanto en `Identity` como en `Web`; el controlador de tareas requiere autorización.

## Brechas conocidas

- **Clave JWT entre servicios:** `Identity` tiene una clave de desarrollo en `appsettings.json`; `Web` requiere `Jwt:Key`, pero su `appsettings.json` solo declara issuer y audience. Configurar una clave común mediante secretos/variables de entorno es necesario para ejecutar y validar tokens entre los dos procesos. No usar la clave de desarrollo fuera de local.
- **Migración de tareas desactualizada:** el modelo actual de Infrastructure ya no incluye `User`, pero la migración inicial y el snapshot de `TaskManagerDbContext` aún definen `Users` y sus relaciones con tareas/comparticiones. Falta generar y revisar una migración que alinee el esquema, además de decidir una transición segura para bases existentes.
- **Base de Identity:** ya existe una migración inicial para `IdentityDbContext`; falta documentar y verificar cómo se aplica y cómo se aprovisionan ambas bases (`TaskManagerIdentityDb` y `TaskManagerDb`) por entorno.
- **Contrato de tareas:** `GET /api/tasks/{taskId}` devuelve actualmente `200` con `null` cuando no encuentra la tarea; conviene responder `404`, como ya hacen actualización y eliminación.
- **Pruebas:** no hay proyectos de pruebas visibles. Hace falta cubrir registro/login, validaciones, emisión y validación de JWT, protección de rutas y aislamiento de tareas por usuario.
- **Funciones del producto:** la API no expone cambio de estado; compartición funcional con permisos y ejecución de tareas programadas aún están pendientes.

## Qué continuar, en orden

1. **Hacer funcionar JWT de extremo a extremo:** configurar la misma clave segura, issuer y audience para `Identity` y `Web` mediante secretos o variables de entorno; verificar registro, login y una llamada autenticada a `/api/tasks`.
2. **Alinear y validar bases de datos:** crear/revisar la migración de `TaskManagerDbContext` para retirar las relaciones heredadas a `Users`; documentar y verificar la aplicación de ambas migraciones y preservar datos existentes.
3. **Cerrar el contrato de API:** cambiar `GetById` para que responda `404` cuando no exista la tarea y normalizar respuestas de validación/conflicto.
4. **Añadir pruebas automatizadas:** probar autenticación, claims, usuarios anónimos, autorización y aislamiento por propietario; añadir pruebas de integración para persistencia.
5. **Completar el flujo de tareas:** exponer cambio de estado; después implementar compartición y hacer cumplir permisos de propietario/colaborador.
6. **Implementar tareas programadas** y decidir si necesitan procesamiento de fondo y notificaciones.
7. Priorizar perfil/preferencias, dashboards e internacionalización según las necesidades del producto; diferir OData y más microservicios hasta que exista una necesidad concreta.

## Alcance aún no completado

Configuración segura compartida de JWT, migración del esquema de tareas tras retirar `User`, aprovisionamiento documentado de bases, respuestas uniformes y pruebas automatizadas, cambio de estado vía API, compartición funcional, ejecución de tareas programadas, notificaciones, perfil/preferencias, dashboards e internacionalización.
