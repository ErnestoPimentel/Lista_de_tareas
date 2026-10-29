# TaskManager

## Estado actual

Solución en desarrollo con proyectos `Domain`, `Application`, `Infrastructure`, `Identity` y `Web`. Identity administra cuentas y emite JWT; Web contiene la API de tareas y la interfaz Blazor Server.

El flujo HTTP de registro/login y la protección JWT de tareas están implementados. También hay validación de credenciales, manejo centralizado de excepciones en Identity, guía de configuración en `README.md` y los primeros proyectos de pruebas. Web ya registra un cliente HTTP para Identity y una sesión de token, aunque todavía no se observan componentes de interfaz que usen esos servicios para completar el inicio de sesión ni envío del token en las solicitudes de tareas desde Blazor.

## Implementado

- Dominio de tareas (`TaskItem`, `TaskShare`, `ScheduledTask`), estados, prioridades y permisos de compartición.
- Persistencia de tareas con EF Core y SQL Server, configuraciones, repositorio, `UnitOfWork` y migración inicial.
- Servicio de aplicación y DTOs para crear, consultar, actualizar y eliminar tareas.
- `TasksController` bajo `api/tasks`, protegido con `[Authorize]`; las operaciones obtienen el usuario del claim `NameIdentifier` y filtran por propietario.
- Proyecto `Identity` con `ApplicationUser`, `IdentityDbContext`, índices únicos para nombre/correo y migración inicial de usuarios.
- Endpoints `POST /api/auth/register` y `POST /api/auth/login`; contraseñas con `IPasswordHasher` y JWT con claims de identificador, nombre y correo.
- Validación de registro: campos requeridos, longitudes, correo válido y contraseña de 12 a 128 caracteres con mayúscula, minúscula y número. Nombre/correo se normalizan y se comprueban duplicados.
- Middleware de excepciones en Identity que devuelve Problem Details: errores de entrada `400`, conflictos `409` y errores no controlados `500` con detalle genérico al cliente.
- Cliente HTTP `IdentityApiClient`, sesión `TokenSession` y registro de ambos servicios en Web como base para conectar la interfaz con Identity.
- `README.md` documenta requisitos, configuración de bases/JWT, migraciones, ejecución local y endpoints.
- SDK .NET fijado en `global.json` a la versión `10.0.403`.
- Proyectos de pruebas en la solución: pruebas unitarias de normalización/reglas de contraseña y pruebas de integración de registro e inicio de sesión de Identity usando `WebApplicationFactory` y base InMemory.

## Brechas conocidas

- **Clave JWT:** Web necesita `Jwt:Key`; Identity conserva una clave de desarrollo en `appsettings.json`. La configuración compartida se explica en README, pero debe usarse un secreto seguro por ambiente y retirar la clave de ejemplo del archivo versionado.
- **Migración de tareas heredada:** el modelo actual de Infrastructure ya no incluye `User`, pero la migración inicial y el snapshot de `TaskManagerDbContext` todavía incluyen `Users` y relaciones con tareas/comparticiones. Falta alinear el esquema y planear una transición segura para bases existentes.
- **Integración de la interfaz:** Web registra `IdentityApiClient` y `TokenSession`, pero no se encontraron usos en componentes Razor/Blazor. Tampoco se ve aún un flujo de login/logout de interfaz ni un mecanismo que adjunte el bearer token a las llamadas protegidas.
- **Manejo de errores en Web:** el middleware centralizado solo está registrado en Identity. La API Web necesita respuestas de error HTTP consistentes para sus propias excepciones.
- **Contrato de tareas:** `GET /api/tasks/{taskId}` responde `200` con `null` si no encuentra la tarea; debería responder `404`.
- **Cobertura de pruebas:** las pruebas actuales cubren reglas de credenciales y registro/login. Faltan pruebas para JWT entre servicios, endpoints protegidos de tareas, aislamiento por usuario, persistencia y flujos de error.
- **Funciones pendientes:** la API no expone cambio de estado; compartir tareas con permisos y ejecutar tareas programadas siguen pendientes.

## Qué continuar, en orden

1. **Completar el flujo de autenticación en Blazor:** crear pantallas/componentes de registro e inicio/cierre de sesión, guardar y limpiar el token de forma adecuada, y enviar el bearer token a las solicitudes protegidas. Definir cómo funcionará la autenticación en el circuito de Blazor Server.
2. **Asegurar y verificar JWT entre hosts:** reemplazar la clave de desarrollo versionada por secretos/variables de entorno comunes y comprobar que Identity emite tokens aceptados por Web.
3. **Alinear la base de tareas:** crear/revisar una migración para eliminar las relaciones heredadas a `Users`, revisar el snapshot, validar instrucciones del README y conservar tareas existentes.
4. **Cerrar el contrato y los errores de Web:** devolver `404` desde `GetById` cuando no hay resultado y definir manejo Problem Details para errores de la API.
5. **Ampliar pruebas automatizadas:** validar JWT y claims, acceso anónimo, autorización de tareas, aislamiento entre usuarios, persistencia y respuestas de error; incluir el flujo de autenticación desde la interfaz cuando exista.
6. **Completar funcionalidad de tareas:** agregar cambio de estado, luego compartir con aplicación efectiva de permisos de propietario/colaborador.
7. Implementar ejecución de tareas programadas y decidir si necesita proceso de fondo y notificaciones.
8. Priorizar perfil/preferencias, dashboards e internacionalización según necesidades; diferir OData y más microservicios hasta que exista una razón concreta.

## Alcance aún no completado

Flujo de autenticación completo en la interfaz Blazor, almacenamiento/configuración segura de JWT, alineación de migraciones de tareas, manejo uniforme de errores en Web, respuesta `404` de consultas individuales, pruebas de autorización e integración Web, cambio de estado por API, compartición funcional, tareas programadas, notificaciones, perfil/preferencias, dashboards e internacionalización.
