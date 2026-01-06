namespace CoursePlatform.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    ILessonRepository Lessons { get; }
    // Users are handled by UserManager, but if we need custom queries:
    // IUserRepository Users { get; } 
    
    Task<int> CompleteAsync();
}
