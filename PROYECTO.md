# TaskManager

## Estado actual

Solución en desarrollo con proyectos separados para `Domain`, `Application`, `Infrastructure`, `Identity` y `Web`. `Identity` administra cuentas y emite tokens; `Web` expone la API de tareas y la aplicación Razor/Blazor.

El flujo de autenticación ya cuenta con endpoints HTTP y el API de tareas valida JWT y exige autenticación. Hay cambios locales en curso en autenticación, validación y configuración; el árbol de trabajo no está limpio.

## Implementado

- Dominio de tareas (`TaskItem`, `TaskShare`, `ScheduledTask`), estados, prioridades y permisos de compartición.
- Persistencia de tareas con EF Core y SQL Server, configuraciones, repositorio, `UnitOfWork` y migración inicial.
- Servicio de aplicación y DTOs para crear, consultar, actualizar y eliminar tareas.
- `TasksController` con rutas CRUD bajo `api/tasks`; las operaciones usan el `UserId` del usuario actual y el repositorio filtra por propietario.
- `ICurrentUser`/`CurrentUser` para leer el identificador de usuario desde claims.
- Proyecto `Identity` con `ApplicationUser`, `IdentityDbContext`, índice único para nombre de usuario y correo, y migración inicial para la tabla `Users`.
- Registro e inicio de sesión en el servicio de autenticación. Las contraseñas se procesan con `IPasswordHasher`; el registro normaliza nombre/correo, aplica anotaciones de validación y detecta usuario/correo duplicados.
- Endpoints `POST /api/auth/register` y `POST /api/auth/login`; emiten JWT con claims de identificador, nombre y correo.
- `Identity` configura validación JWT y autorización. `Web` también configura bearer JWT, middleware de autenticación/autorización y protege el controlador de tareas con `[Authorize]`.

## Cambios locales en curso

- Hay modificaciones sin confirmar en los modelos y servicio de autenticación, validaciones de login/registro, controladores, configuración JWT y dependencias de `Web` e `Identity`.
- Se agregó una migración inicial para `IdentityDbContext`.
- `TaskManager.Web/appsettings.json` declara issuer y audience, pero no contiene `Jwt:Key`; `Identity` sí contiene una clave de desarrollo en `appsettings.json`. La clave compartida debe configurarse de forma segura en ambos procesos para que `Web` valide los tokens que emite `Identity`.

## Brechas conocidas

- La migración/snapshot originales de `TaskManagerDbContext` aún corresponden al esquema anterior con tabla `Users` y relaciones hacia ella, mientras el modelo actual de Infrastructure ya no incluye `User`. Falta alinear y revisar la transición de esa base.
- La migración de Identity existe, pero falta documentar/aplicar el aprovisionamiento de ambas bases (`TaskManagerIdentityDb` y `TaskManagerDb`) en los entornos correspondientes.
- `GET /api/tasks/{taskId}` devuelve `200` con `null` si la tarea no existe; debe responder `404` de forma consistente con actualización y eliminación.
- No hay proyectos de pruebas visibles. Se necesita verificar registro/login, compatibilidad de tokens entre servicios, acceso anónimo y separación de datos por usuario.
- El CRUD no ofrece cambio de estado; compartir tareas, aplicar permisos de compartición y ejecutar tareas programadas no está implementado como flujo completo.
- La clave de desarrollo incluida en configuración no debe usarse en ambientes compartidos o productivos.

## Qué continuar, en orden

1. **Cerrar configuración y seguridad de JWT:** usar el mismo issuer, audience y clave segura en `Identity` y `Web`, proveer la clave mediante secretos/variables por ambiente, y comprobar que ningún secreto real quede versionado.
2. **Probar el recorrido de autenticación de punta a punta:** aplicar la migración de Identity, registrar e iniciar sesión, enviar el bearer token a `Web` y confirmar que acepta el token y obtiene el `UserId` correcto.
3. **Alinear persistencia de tareas:** actualizar el snapshot y generar una migración que quite las relaciones antiguas con `Users`; decidir cómo actualizar bases existentes sin perder tareas.
4. **Completar el contrato HTTP:** devolver `404` en `GetById`, estandarizar errores de validación y conflictos, y confirmar las respuestas `201`, `200`, `401` y `409` de autenticación.
5. **Añadir pruebas automatizadas:** cubrir validación de requests, hashing, emisión/validación de JWT, protección de endpoints y aislamiento por `UserId`.
6. **Completar funciones del producto:** exponer cambio de estado; después implementar compartición con sus permisos y validar acceso del propietario/colaborador.
7. **Implementar tareas programadas** y decidir si requieren servicio de fondo y notificaciones.
8. **Priorizar funciones posteriores** como perfil/preferencias, dashboards e internacionalización según necesidades; diferir OData y microservicios adicionales hasta que exista una necesidad concreta.

## Alcance aún no completado

Configuración segura común de JWT, transición de migraciones de tareas, aprovisionamiento documentado de bases, manejo consistente de respuestas y errores, pruebas automatizadas, cambio de estado vía API, compartición funcional, ejecución de tareas programadas, notificaciones, perfil/preferencias, dashboards e internacionalización.
