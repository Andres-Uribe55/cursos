# Plataforma de Gestión de Cursos (CoursePlatform)

Un sistema robusto y escalable para la gestión de cursos y lecciones, construido con una arquitectura moderna y segura utilizando .NET 8 y Angular 18.

[Ver Diagramas de Arquitectura y Base de Datos](./docs/DIAGRAMS.md)

## 🚀 Arquitectura del Sistema

El proyecto sigue los principios de **Clean Architecture** y patrones de diseño avanzados:
- **Capa de Dominio**: Entidades, excepciones y lógica pura de negocio.
- **Capa de Aplicación**: Interfaces, DTOs y servicios de orquestación.
- **Capa de Infraestructura**: Persistencia (EF Core), Repositorios, Unit of Work y Seguridad (JWT/Refresh Tokens).
- **Capa de API**: Controladores REST, Middleware de excepciones profesional y configuración de Swagger.

---

## ⚙️ Configuración y Ejecución

### 📋 Requisitos Previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/) (o Docker para levantarlo rápidamente)

### 🗄️ 1. Configuración de la Base de Datos
1. Asegúrate de tener una instancia de PostgreSQL ejecutándose.
2. Abre el archivo `curso-backend/src/CoursePlatform.API/appsettings.json`.
3. Actualiza el valor de `DefaultConnection` con tus credenciales de PostgreSQL:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=courseplatform;Username=tu_usuario;Password=tu_password"
   }
   ```

### 🔄 2. Comandos para Migraciones
El sistema está configurado para auto-aplicar migraciones al iniciar, pero si deseas hacerlo manualmente desde la consola:
1. Ve a la carpeta raíz del backend: `cd curso-backend`
2. Ejecuta los siguientes comandos:
   ```bash
   # Instalar herramientas de EF si no las tienes
   dotnet tool install --global dotnet-ef

   # Aplicar la última migración a la base de datos
   dotnet ef database update --project src/CoursePlatform.Infrastructure --startup-project src/CoursePlatform.API
   ```

### ⚡ 3. Ejecución de la API y Frontend

#### Backend (API)
1. Navega a `curso-backend/src/CoursePlatform.API`
2. Ejecuta: `dotnet run`
3. La API estará disponible en `http://localhost:5207` (puedes ver la documentación en `/swagger`).

#### Frontend (Angular)
1. Navega a `curso-frontend`
2. Instala dependencias: `npm install`
3. Ejecuta: `npm start`
4. Accede desde tu navegador a `http://localhost:4200`.

---

## 🐳 Despliegue con Docker (Recomendado)
Si prefieres no instalar dependencias locales, usa Docker Compose desde la raíz:
```bash
docker compose up --build -d
```
Esto levantará automáticamente la base de datos (puerto 5433), la API (puerto 5207) y el Frontend (puerto 4200).

---

## 🔒 Credenciales de Acceso (Seed Data)

Utiliza estas cuentas para probar las funcionalidades de cada rol:

| Rol | Email | Contraseña |
|-----|-------|------------|
| **Administrador** | admin@example.com | Test123! |
| **Estudiante** | student@example.com | Test123! |

---

## 📂 Estructura del Código
- **backend**: Implementa el patrón Repository y Unit of Work.
- **frontend**: Implementa interceptores de seguridad para auto-renovación de tokens y guardas de autenticación.
- **pruebas**: Contiene tests unitarios para las reglas de negocio críticas.
