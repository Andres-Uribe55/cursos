using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Application.Interfaces;

public interface ILessonRepository : IGenericRepository<Lesson>
{
    Task<int> GetMaxOrderAsync(Guid courseId);
}
