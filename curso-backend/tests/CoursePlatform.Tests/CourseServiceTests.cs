using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Services;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Exceptions;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CoursePlatform.Tests;

public class TestUnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public TestUnitOfWork(DbContext context)
    {
        _context = context;
        Courses = new CourseRepository(_context);
        Lessons = new LessonRepository(_context);
    }

    public ICourseRepository Courses { get; }
    public ILessonRepository Lessons { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

public class CourseServiceTests
{
    private DbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new TestDbContext(options);
        return context;
    }

    [Fact]
    public async Task PublishCourse_WithLessons_ShouldSucceed()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        var lessonService = new LessonService(uow);

        var courseDto = await courseService.CreateAsync(new CreateCourseDto("Test Course", "Desc", "http://url"));
        await lessonService.CreateAsync(new CreateLessonDto(courseDto.Id, "Lesson 1", 1));

        // Act
        await courseService.PublishAsync(courseDto.Id);

        // Assert
        var course = await context.Set<Course>().FindAsync(courseDto.Id);
        Assert.NotNull(course);
        Assert.Equal(CourseStatus.Published, course.Status);
    }

    [Fact]
    public async Task PublishCourse_WithoutLessons_ShouldFail()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);

        var courseDto = await courseService.CreateAsync(new CreateCourseDto("Test Course", "Desc", "url"));

        // Act & Assert
        await Assert.ThrowsAsync<CannotPublishCourseException>(
            async () => await courseService.PublishAsync(courseDto.Id)
        );
    }

    [Fact]
    public async Task CreateLesson_WithUniqueOrder_ShouldSucceed()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        var lessonService = new LessonService(uow);

        var courseDto = await courseService.CreateAsync(new CreateCourseDto("Test Course", "Desc", "url"));

        // Act
        var lessonDto = await lessonService.CreateAsync(new CreateLessonDto(courseDto.Id, "Lesson 1", 1));

        // Assert
        Assert.NotNull(lessonDto);
        Assert.Equal(1, lessonDto.Order);
    }

    [Fact]
    public async Task CreateLesson_WithDuplicateOrder_ShouldFail()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        var lessonService = new LessonService(uow);

        var courseDto = await courseService.CreateAsync(new CreateCourseDto("Test Course", null, null));
        await lessonService.CreateAsync(new CreateLessonDto(courseDto.Id, "Lesson 1", 1));

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateLessonOrderException>(
            async () => await lessonService.CreateAsync(new CreateLessonDto(courseDto.Id, "Lesson 2", 1))
        );
    }

    [Fact]
    public async Task DeleteCourse_ShouldBeSoftDelete()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);

        var courseDto = await courseService.CreateAsync(new CreateCourseDto("Test Course", null, null));

        // Act
        await courseService.DeleteAsync(courseDto.Id);

        // Assert
        var course = await context.Set<Course>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == courseDto.Id);
        
        Assert.NotNull(course);
        Assert.True(course.IsDeleted);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnCourse()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        var created = await courseService.CreateAsync(new CreateCourseDto("Test Course", null, null));

        // Act
        var result = await courseService.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Test Course", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFound()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);

        // Act & Assert
        await Assert.ThrowsAsync<CourseNotFoundException>(
            async () => await courseService.GetByIdAsync(Guid.NewGuid())
        );
    }

    [Fact]
    public async Task UpdateAsync_WithValidDetails_ShouldUpdateTitle()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        var created = await courseService.CreateAsync(new CreateCourseDto("Original Title", null, null));

        // Act
        var updated = await courseService.UpdateAsync(created.Id, new UpdateCourseDto("Updated Title", null, null));

        // Assert
        Assert.Equal("Updated Title", updated.Title);
        
        var inDb = await context.Set<Course>().FindAsync(created.Id);
        Assert.NotNull(inDb);
        Assert.Equal("Updated Title", inDb.Title);
    }

    [Fact]
    public async Task UnpublishAsync_PublishedCourse_ShouldChangeToDraft()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        var lessonService = new LessonService(uow);
        
        var created = await courseService.CreateAsync(new CreateCourseDto("Test Course", null, null));
        await lessonService.CreateAsync(new CreateLessonDto(created.Id, "Lesson 1", 1));
        await courseService.PublishAsync(created.Id);

        // Act
        await courseService.UnpublishAsync(created.Id);

        // Assert
        var course = await context.Set<Course>().FindAsync(created.Id);
        Assert.NotNull(course);
        Assert.Equal(CourseStatus.Draft, course.Status);
    }

    [Fact]
    public async Task SearchAsync_NoParams_ShouldReturnDefaultPageSize()
    {
        // Arrange
        var context = CreateInMemoryContext();
        using var uow = new TestUnitOfWork(context);
        var courseService = new CourseService(uow);
        
        // Create 15 courses
        for (int i = 0; i < 15; i++)
        {
            await courseService.CreateAsync(new CreateCourseDto($"Course {i}", null, null));
        }

        // Act
        var result = await courseService.SearchAsync(new CourseSearchParams(null, null));

        // Assert
        Assert.Equal(10, result.Items.Count()); // Default page size is 10
        Assert.Equal(15, result.TotalCount);
    }
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasQueryFilter(e => !e.IsDeleted);
            
            entity.HasMany(e => e.Lessons)
                  .WithOne(e => e.Course)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}