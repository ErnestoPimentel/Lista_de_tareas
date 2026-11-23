# TaskManager

## Estado actual

Solución en desarrollo sobre .NET 10. Incluye una API de tareas, una interfaz Blazor Server y un servicio separado de identidad. El flujo de registro e inicio de sesión está conectado a la interfaz y usa JWT.

### Proyectos

| Proyecto | Responsabilidad |
| --- | --- |
| `TaskManager.Domain` | Entidades y enums del dominio de tareas. |
| `TaskManager.Application` | Servicios, DTOs e interfaces de aplicación. |
| `TaskManager.Infrastructure` | Persistencia SQL Server, Entity Framework Core y repositorios. |
| `TaskManager.Contracts` | Requests y responses compartidos entre Web e Identity. |
| `TaskManager.Identity` | Registro, inicio de sesión, validación de credenciales y emisión de JWT. |
| `TaskManager.Web` | Interfaz Blazor Server y API protegida de tareas. |
| `TaskManager.Identity.UnitTests` | Pruebas unitarias de reglas de credenciales. |
| `TaskManager.Identity.IntegrationTests` | Pruebas HTTP de registro e inicio de sesión. |

## Funcionalidad implementada

### Tareas

- Crear, consultar, actualizar y eliminar tareas.
- Filtrar las operaciones por el identificador del usuario autenticado.
- Persistir tareas, comparticiones y tareas programadas con EF Core y SQL Server.
- Usar repositorio, `UnitOfWork` y servicios de Application.

### Identidad e interfaz

- Registrar usuarios e iniciar sesión mediante `POST /api/auth/register` y `POST /api/auth/login`.
- Guardar contraseñas usando `IPasswordHasher` y validar los datos de registro.
- Emitir JWT con claims de identificador, nombre y correo.
- Validar JWT y proteger la API de tareas con `[Authorize]`.
- Mostrar páginas Blazor de registro e inicio de sesión, estado de autenticación y menú para iniciar/cerrar sesión.
- Compartir modelos de autenticación mediante `TaskManager.Contracts`.
- Manejar errores de forma centralizada en el host de Identity.

### Pruebas disponibles

- Pruebas unitarias de normalización de nombre y correo, y de reglas de contraseña.
- Pruebas de integración de Identity para registro correcto, inicio de sesión correcto y contraseña incorrecta.
- Las pruebas de integración de Identity usan `WebApplicationFactory` y una base InMemory.

## Limitaciones y pendientes conocidos

- **La sesión del navegador no es persistente.** `TokenSession` guarda el token en memoria; al finalizar el circuito de Blazor o crear uno nuevo, el usuario puede perder la sesión.
- **La interfaz aún no consume la API de tareas.** `IdentityApiClient` llama a Identity para registrar e iniciar sesión, pero no se observa una interfaz de tareas que envíe el bearer token a los endpoints protegidos.
- **La clave JWT es de desarrollo y está en configuración versionada.** Debe sustituirse por secretos o variables de entorno para cada ambiente.
- **El esquema de tareas conserva referencias antiguas a usuarios.** La migración inicial y el snapshot de `TaskManagerDbContext` incluyen `Users` y claves foráneas que ya no están en el modelo actual de Infrastructure.
- **La API responde `200` con `null` en una consulta sin resultados.** `GET /api/tasks/{taskId}` debería devolver `404` cuando la tarea no existe.
- **El manejo centralizado de excepciones está solo en Identity.** Web aún requiere respuestas de error consistentes para su API.
- **La cobertura de pruebas es parcial.** No hay pruebas de autorización de tareas, aislamiento entre usuarios, persistencia de tareas ni del flujo de autenticación completo desde la interfaz.
- No se ha implementado el cambio de estado por API, la compartición efectiva de tareas ni la ejecución de tareas programadas.

## Próximos pasos recomendados

1. **Conectar Blazor con la API de tareas:** implementar llamadas autenticadas que adjunten el JWT y crear la experiencia para listar y administrar tareas.
2. **Definir la duración y persistencia de sesión:** decidir cómo restaurar, expirar y cerrar sesión de forma coherente con Blazor Server.
3. **Mover la clave JWT a configuración segura:** usar el mismo secreto, issuer y audience en Identity y Web mediante configuración por ambiente.
4. **Alinear el esquema de tareas:** crear y revisar una migración que quite las referencias heredadas a `Users`; definir cómo actualizar bases existentes sin perder tareas.
5. **Completar el contrato de la API:** devolver `404` desde `GetById` cuando no exista la tarea y estandarizar el manejo de errores HTTP en Web.
6. **Ampliar las pruebas:** cubrir JWT, rutas protegidas, aislamiento de datos por usuario, persistencia y errores de API.
7. **Completar las funciones de tareas:** agregar cambio de estado y después implementar compartición con aplicación de permisos.
8. **Implementar tareas programadas** y determinar si requieren procesamiento en segundo plano y notificaciones.
9. Priorizar perfil, preferencias, dashboards e internacionalización según las necesidades del producto.

## Alcance futuro

Compartición funcional, tareas programadas y notificaciones, perfil y preferencias, dashboards e internacionalización. OData y más microservicios pueden evaluarse cuando exista una necesidad concreta.
