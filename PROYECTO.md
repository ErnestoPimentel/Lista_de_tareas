### Completado

- Definición inicial del proyecto.
- Definición del alcance funcional.
- Definición de tecnologías.
- Definición inicial de arquitectura.
- Definición inicial de módulos.
- Definición de estructura de solución.
- Creación de la solución TaskManager.
- Creación de TaskManager.Domain.
- Creación de TaskManager.Application.
- Creación de TaskManager.Infrastructure.
- Creación de TaskManager.Web.
- Configuración inicial de referencias entre proyectos.
- Definición inicial del dominio.
- Creación de la entidad base Entity.
- Creación de la entidad User.
- Creación de la entidad TaskItem.
- Creación de la entidad TaskShare.
- Creación de la entidad ScheduledTask.
- Definición de estados de tareas.
- Definición de prioridades.
- Definición de permisos para tareas compartidas.
- Configuración inicial de Entity Framework Core.
- Configuración de SQL Server.
- Creación de TaskManagerDbContext.
- Configuración independiente de las entidades mediante IEntityTypeConfiguration.
- Configuración de User.
- Configuración de TaskItem.
- Configuración de TaskShare.
- Configuración de ScheduledTask.
- Configuración inicial de inyección de dependencias de Infrastructure.
- Registro de Infrastructure en la aplicación Web.
- Configuración inicial de la cadena de conexión.
- Integración de TaskManagerDbContext con ASP.NET Core.
- Creación de la migración inicial de Entity Framework Core.
- Creación inicial de la base de datos TaskManagerDb.
- Generación inicial de las tablas del dominio.
- Implementación de ITaskRepository.
- Implementación de TaskRepository mediante Entity Framework Core.
- Implementación asíncrona de las operaciones de persistencia.
- Registro de TaskRepository mediante inyección de dependencias.
- Separación de persistencia respecto a la capa Application.
- Creación de la abstracción IUnitOfWork.
- Implementación de UnitOfWork.
- Separación de persistencia y confirmación de cambios.
- Integración de UnitOfWork con TaskService.
- Preparación para operaciones transaccionales entre múltiples entidades.

### En desarrollo

- API de tareas.
- Autenticación y autorización.
- Middleware y manejo de errores.

### Pendiente

- Autenticación.
- Autorización.
- DTOs.
- APIs.
- OData.
- Microservicios.
- Internacionalización.
- Notificaciones.
- Programación de tareas.
- Dashboards.
- Configuración de usuario.
- Configuración global.
- Pruebas.