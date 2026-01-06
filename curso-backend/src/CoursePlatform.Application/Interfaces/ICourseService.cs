using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Interfaces;

public interface ICourseService
{
    Task<PaginatedResponse<CourseDto>> SearchAsync(CourseSearchParams searchParams);
    Task<CourseDto> GetByIdAsync(Guid id);
    Task<CourseSummaryDto> GetSummaryAsync(Guid id);
    Task<CourseDto> CreateAsync(CreateCourseDto dto);
    Task<CourseDto> UpdateAsync(Guid id, UpdateCourseDto dto);
    Task DeleteAsync(Guid id);
    Task PublishAsync(Guid id);
    Task UnpublishAsync(Guid id);
}