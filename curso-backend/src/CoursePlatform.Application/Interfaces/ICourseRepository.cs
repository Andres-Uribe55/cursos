using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Application.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
    // Add specific methods if needed, e.g.
    Task<IEnumerable<Course>> GetCoursesWithLessonsAsync(int page, int pageSize);
    Task<(IEnumerable<Course> Items, int TotalCount)> SearchAsync(string? q, CourseStatus? status, int page, int pageSize);
}
