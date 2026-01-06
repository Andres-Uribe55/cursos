namespace CoursePlatform.Application.DTOs;

public record LessonDto(
    Guid Id,
    Guid CourseId,
    string Title,
    int Order,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateLessonDto(
    Guid CourseId,
    string Title,
    int Order
);

public record UpdateLessonDto(
    string Title,
    int Order
);

public record ReorderLessonDto(
    Guid LessonId,
    int NewOrder
);