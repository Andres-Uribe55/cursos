# Plataforma de Gestión de Cursos (CoursePlatform)

Un sistema robusto y escalable para la gestión de cursos y lecciones, construido con una arquitectura moderna y segura.

## 🚀 Arquitectura del Sistema

El proyecto sigue los principios de **Clean Architecture** y patrones de diseño avanzados para asegurar que el código sea mantenible, testeable y desacoplado.

### Backend (.NET 8 Core)
- **Patrón Repositorio y Unit of Work**: Abstracción total de la capa de datos. El `IUnitOfWork` coordina las transacciones y el acceso a repositorios específicos (`ICourseRepository`, `ILessonRepository`).
- **Seguridad Avanzada**:
  - Autenticación JWT con **Refresh Tokens** persistentes y rotación automática.
  - Gestión de roles (Admin, Student) con endpoints y controladores diferenciados.
  - Middleware de manejo de excepciones profesional que retorna `ProblemDetails` (RFC 7807).
- **Modelado de Datos**:
  - Filtros de consulta globales para **Soft Delete** automático en `Course` y `Lesson`.
  - Índices de base de datos optimizados para búsquedas frecuentes.
  - Seed data completo y realista que incluye usuarios, roles, cursos y lecciones iniciales.
- **Calidad y Mantenibilidad**:
  - Pruebas unitarias con xUnit verificando reglas de negocio complejas.
  - Código desacoplado mediante Inyección de Dependencias.
  - Documentación OpenAPI (Swagger) integrada.

### Frontend (Angular)
- **Diseño Responsivo y Premium**: Interfaz moderna con Tabbed UI para gestión de perfil.
- **Interceptores de Seguridad**: El `AuthInterceptor` gestiona automáticamente la inserción de tokens y la renovación mediante refresh tokens ante errores 401.
- **Gestión de Perfil**: Panel de control para que el usuario gestione sus datos personales y seguridad.

## 🛠️ Tecnologías

- **Backend**: ASP.NET Core 8, Entity Framework Core, PostgreSQL, ASP.NET Core Identity.
- **Frontend**: Angular 18+, RXJS, Standalone Components.

## ⚙️ Configuración y Ejecución

### Requisitos
- .NET 8 SDK
- Node.js 20+
- Servidor PostgreSQL activo

### Ejecución del Backend
1. Ir al directorio: `curso-backend/src/CoursePlatform.API`
2. El sistema auto-aplicará migraciones y cargará seed data al iniciar.
3. Comando: `dotnet run`

### Ejecución del Frontend
1. Ir al directorio: `curso-frontend`
2. Instalar: `npm install`
3. Comando: `npm start`

## 📂 Estructura del Código

- **src/CoursePlatform.Domain**: Entidades núcleo del negocio.
- **src/CoursePlatform.Application**: Contratos de servicios, DTOs y lógica de aplicación.
- **src/CoursePlatform.Infrastructure**: Implementación de repositorios, base de datos y servicios de infraestructura.
- **src/CoursePlatform.API**: Entrada del sistema, controladores y middleware.

## 🔒 Credenciales de Acceso (Seed)

| Rol | Email | Contraseña |
|-----|-------|------------|
| Administrador | admin@example.com | Test123! |
| Estudiante | student@example.com | Test123! |
