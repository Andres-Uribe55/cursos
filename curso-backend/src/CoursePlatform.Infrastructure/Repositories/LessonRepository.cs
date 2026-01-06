using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Repositories;

public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
{
    public LessonRepository(DbContext context) : base(context)
    {
    }

    public async Task<int> GetMaxOrderAsync(Guid courseId)
    {
        // Use generic database set but context is easier for specific queries
        var maxOrder = await dbSet
            .Where(l => l.CourseId == courseId && !l.IsDeleted)
            .MaxAsync(l => (int?)l.Order);
            
        return maxOrder ?? 0;
    }
}
