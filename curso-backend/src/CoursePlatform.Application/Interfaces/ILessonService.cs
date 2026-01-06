using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Interfaces;

public interface ILessonService
{
    Task<List<LessonDto>> GetByCourseIdAsync(Guid courseId);
    Task<LessonDto> GetByIdAsync(Guid id);
    Task<LessonDto> CreateAsync(CreateLessonDto dto);
    Task<LessonDto> UpdateAsync(Guid id, UpdateLessonDto dto);
    Task DeleteAsync(Guid id);
    Task MoveUpAsync(Guid id);
    Task MoveDownAsync(Guid id);
}