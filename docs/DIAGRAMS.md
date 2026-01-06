# Diagramas Técnicos - CoursePlatform

Este documento contiene la representación visual de la base de datos y la arquitectura de software del proyecto.

## 📊 Diagrama de Entidad-Relación (ERD)
Representa la estructura de persistencia en PostgreSQL.

![Diagrama de Entidad Relación](./images/erd.png)

```mermaid
erDiagram
    USER ||--o{ REFRESH_TOKEN : "posee"
    COURSE ||--o{ LESSON : "contiene"
    
    USER {
        string Id PK
        string FirstName
        string LastName
        string Email
        int Age
        datetime CreatedAt
    }
    
    REFRESH_TOKEN {
        guid Id PK
        string Token
        string JwtId
        datetime ExpiryDate
        bool Used
        bool Invalidated
        string UserId FK
    }
    
    COURSE {
        guid Id PK
        string Title
        string Description
        string Status
        bool IsDeleted
        datetime CreatedAt
    }
    
    LESSON {
        guid Id PK
        string Title
        int Order
        bool IsDeleted
        guid CourseId FK
    }
```

---

## 🏗️ Diagrama de Clases (Arquitectura)
Muestra la implementación del patrón Repository, Unit of Work y la capa de Servicios.

![Diagrama de Clases](./images/class_diagram.png)

```mermaid
classDiagram
    class IUnitOfWork {
        <<interface>>
        +ICourseRepository Courses
        +ILessonRepository Lessons
        +CompleteAsync() Task
    }
    
    class ICourseService {
        <<interface>>
        +GetByIdAsync(Guid id)
        +PublishAsync(Guid id)
    }

    class CourseService {
        -IUnitOfWork _uow
        +PublishAsync(Guid id)
    }

    class GenericRepository~T~ {
        +GetByIdAsync(Guid id)
        +AddAsync(T entity)
    }

    class CourseRepository {
        +SearchAsync()
    }

    CourseService ..|> ICourseService
    CourseService --> IUnitOfWork
    CourseRepository --|> GenericRepository~Course~
    IUnitOfWork --> ICourseRepository
    IUnitOfWork --> ILessonRepository
```
