using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(DbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Course>> GetCoursesWithLessonsAsync(int page, int pageSize)
    {
        return await dbSet
            .Include(c => c.Lessons)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Course> Items, int TotalCount)> SearchAsync(string? q, CourseStatus? status, int page, int pageSize)
    {
        var query = dbSet.Include(c => c.Lessons).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(c => c.Title.Contains(q));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
