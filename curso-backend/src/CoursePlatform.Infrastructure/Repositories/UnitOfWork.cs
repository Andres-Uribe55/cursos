using CoursePlatform.Application.Interfaces;
using CoursePlatform.Infrastructure.Data;

namespace CoursePlatform.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Courses = new CourseRepository(_context);
        Lessons = new LessonRepository(_context);
    }

    public ICourseRepository Courses { get; private set; }
    public ILessonRepository Lessons { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
