# TaskManager

## Estado actual

Solución en desarrollo con proyectos separados para `Domain`, `Application`, `Infrastructure`, `Identity` y `Web`. `Identity` administra cuentas y emite JWT; `Web` contiene la API autenticada de tareas y la aplicación Razor/Blazor.

El registro/login y la validación JWT están implementados entre ambos hosts. También se añadieron validación de credenciales, normalización de nombre/correo, manejo centralizado de excepciones en Identity y un `README.md` con configuración, ejecución, migraciones y ejemplos de API. El repositorio está limpio en el commit `bd3c247`.

## Implementado

- Dominio de tareas (`TaskItem`, `TaskShare`, `ScheduledTask`), estados, prioridades y permisos de compartición.
- Persistencia de tareas con EF Core y SQL Server, configuraciones, repositorio, `UnitOfWork` y migración inicial.
- Servicio de aplicación y DTOs para crear, consultar, actualizar y eliminar tareas.
- `TasksController` bajo `api/tasks`, protegido con `[Authorize]`; las operaciones obtienen el usuario de `NameIdentifier` y filtran por propietario.
- Proyecto `Identity` con `ApplicationUser`, `IdentityDbContext`, índices únicos para nombre/correo y migración inicial de `Users`.
- Endpoints `POST /api/auth/register` y `POST /api/auth/login`; hashing de contraseñas con `IPasswordHasher` y emisión de JWT con claims de identificador, nombre y correo.
- Validación de registro: campos requeridos, longitudes, formato de correo y contraseña de al menos 12 caracteres con mayúscula, minúscula y número. Nombre de usuario y correo se normalizan antes de guardar; se detectan duplicados.
- `ExceptionHandlingMiddleware` en Identity: traduce argumentos inválidos a `400`, conflictos a `409` y errores no controlados a `500` con respuestas Problem Details y registro de errores internos.
- Guía en `README.md` con requisitos, configuración de las dos bases, variables JWT compartidas, comandos de migración, ejecución local y ejemplos de endpoints.

## Brechas conocidas

- **Configuración de JWT:** Web requiere `Jwt:Key`, mientras Identity tiene una clave de desarrollo versionada. El README explica cómo proporcionar un secreto común por ambiente; falta retirar la clave insegura del archivo de configuración y comprobar la integración con un secreto local seguro.
- **Migración de tareas heredada:** el modelo actual de Infrastructure ya no incluye `User`, pero su migración inicial y snapshot aún definen `Users` y relaciones con tareas/comparticiones. Falta alinear el esquema y definir una transición segura para bases existentes.
- **Manejo de errores en Web:** el middleware nuevo solo se registra en Identity. Web mantiene su manejador de errores de interfaz para producción y no tiene un manejador API equivalente para excepciones de sus controladores.
- **Contrato de tareas:** `GET /api/tasks/{taskId}` devuelve actualmente `200` con `null` si no existe la tarea; debe responder `404` para ser consistente con actualización/eliminación.
- **Pruebas:** no hay proyectos de pruebas visibles. Faltan pruebas de validación, autenticación, autorización, aislamiento por usuario y persistencia.
- **Funciones pendientes:** la API no expone cambio de estado; compartir tareas con aplicación de permisos y ejecutar tareas programadas aún no está implementado.

## Qué continuar, en orden

1. **Asegurar y verificar JWT:** quitar la clave de ejemplo de `Identity/appsettings.json`, configurar una clave común mediante secretos/variables de entorno en ambos servicios y comprobar registro, login y una solicitud autenticada a `/api/tasks`.
2. **Alinear bases de datos:** generar y revisar la migración de `TaskManagerDbContext` para retirar las relaciones heredadas a `Users`; validar los comandos del README y preservar los datos de bases existentes.
3. **Completar respuestas de la API de tareas:** responder `404` en `GetById` y añadir manejo de errores HTTP coherente en Web sin convertir errores internos en detalles expuestos al cliente.
4. **Añadir pruebas automatizadas:** cubrir reglas de credenciales, hashing, emisión/validación de JWT, rutas anónimas, claims e aislamiento por propietario; incluir integración con persistencia.
5. **Completar el flujo funcional de tareas:** exponer cambio de estado y después implementar compartición haciendo cumplir permisos de propietario y colaborador.
6. **Implementar tareas programadas** y decidir si requieren procesamiento en segundo plano y notificaciones.
7. Priorizar perfil/preferencias, dashboards e internacionalización según necesidades; diferir OData y más microservicios hasta que exista una razón concreta.

## Alcance aún no completado

Secreto JWT seguro fuera del repositorio, migración de tareas alineada con el modelo actual, estrategia de transición y aprovisionamiento de bases documentada, manejo centralizado de errores para la API Web, respuesta `404` en consultas individuales, pruebas automatizadas, cambio de estado vía API, compartición funcional, ejecución de tareas programadas, notificaciones, perfil/preferencias, dashboards e internacionalización.
