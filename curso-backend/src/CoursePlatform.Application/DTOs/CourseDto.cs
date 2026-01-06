using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Application.DTOs;

public record CourseDto(
    Guid Id,
    string Title,
    string Description,
    string ResourceUrl,
    CourseStatus Status,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateCourseDto(string Title, string? Description = null, string? ResourceUrl = null);

public record UpdateCourseDto(string Title, string? Description = null, string? ResourceUrl = null);

public record CourseSummaryDto(
    Guid Id,
    string Title,
    string Description,
    string ResourceUrl,
    CourseStatus Status,
    int TotalLessons,
    DateTime LastModified
);

public record CourseSearchParams(
    string? Q,
    CourseStatus? Status,
    int Page = 1,
    int PageSize = 10
);

public record PaginatedResponse<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);