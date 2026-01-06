using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Services;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;

    public LessonService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<LessonDto>> GetByCourseIdAsync(Guid courseId)
    {
        var lessons = await _unitOfWork.Lessons.FindAsync(
            filter: l => l.CourseId == courseId,
            orderBy: q => q.OrderBy(l => l.Order)
        );

        return lessons.Select(l => new LessonDto(
            l.Id,
            l.CourseId,
            l.Title,
            l.Order,
            l.IsDeleted,
            l.CreatedAt,
            l.UpdatedAt
        )).ToList();
    }

    public async Task<LessonDto> GetByIdAsync(Guid id)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        return new LessonDto(
            lesson.Id,
            lesson.CourseId,
            lesson.Title,
            lesson.Order,
            lesson.IsDeleted,
            lesson.CreatedAt,
            lesson.UpdatedAt
        );
    }

    public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
        if (course == null)
            throw new CourseNotFoundException(dto.CourseId);

        if (dto.Order > 0)
        {
            var existingLesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(
                l => l.CourseId == dto.CourseId && l.Order == dto.Order && !l.IsDeleted);

            if (existingLesson != null)
                throw new DuplicateLessonOrderException(dto.Order);
        }
        else
        {
            // Auto-assign next order
            var maxOrder = await _unitOfWork.Lessons.GetMaxOrderAsync(dto.CourseId);
            
            dto = dto with { Order = maxOrder + 1 };
        }

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = dto.CourseId,
            Title = dto.Title,
            Order = dto.Order,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Lessons.AddAsync(lesson);
        
        course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(course);
        
        await _unitOfWork.CompleteAsync();

        return new LessonDto(
            lesson.Id,
            lesson.CourseId,
            lesson.Title,
            lesson.Order,
            lesson.IsDeleted,
            lesson.CreatedAt,
            lesson.UpdatedAt
        );
    }

    public async Task<LessonDto> UpdateAsync(Guid id, UpdateLessonDto dto)
    {
        var lesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(l => l.Id == id, "Course");
        if (lesson == null)
            throw new LessonNotFoundException(id);

        if (lesson.Order != dto.Order)
        {
            var existingLesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(
                l => l.CourseId == lesson.CourseId && l.Order == dto.Order && l.Id != id && !l.IsDeleted);

            if (existingLesson != null)
                throw new DuplicateLessonOrderException(dto.Order);
        }

        lesson.Title = dto.Title;
        lesson.Order = dto.Order;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        lesson.Course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(lesson.Course); // Track course update
        _unitOfWork.Lessons.Update(lesson);

        await _unitOfWork.CompleteAsync();

        return new LessonDto(
            lesson.Id,
            lesson.CourseId,
            lesson.Title,
            lesson.Order,
            lesson.IsDeleted,
            lesson.CreatedAt,
            lesson.UpdatedAt
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        var lesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(l => l.Id == id, "Course");
        if (lesson == null)
            throw new LessonNotFoundException(id);

        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        lesson.Course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(lesson.Course);
        _unitOfWork.Lessons.Update(lesson);

        await _unitOfWork.CompleteAsync();
    }

    public async Task MoveUpAsync(Guid id)
    {
        var lesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(l => l.Id == id, "Course");
        if (lesson == null)
            throw new LessonNotFoundException(id);

        if (lesson.Order <= 1)
            throw new DomainException("Cannot move up. Lesson is already at the top.");

        var previousLesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(
            l => l.CourseId == lesson.CourseId && l.Order == lesson.Order - 1 && !l.IsDeleted);

        if (previousLesson != null)
        {
            previousLesson.Order++;
            previousLesson.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Lessons.Update(previousLesson);
        }

        lesson.Order--;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        lesson.Course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(lesson.Course);
        _unitOfWork.Lessons.Update(lesson);

        await _unitOfWork.CompleteAsync();
    }

    public async Task MoveDownAsync(Guid id)
    {
        var lesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(l => l.Id == id, "Course");
        if (lesson == null)
            throw new LessonNotFoundException(id);

        var maxOrder = await _unitOfWork.Lessons.GetMaxOrderAsync(lesson.CourseId);

        if (lesson.Order >= maxOrder)
            throw new DomainException("Cannot move down. Lesson is already at the bottom.");

        var nextLesson = await _unitOfWork.Lessons.GetFirstOrDefaultAsync(
            l => l.CourseId == lesson.CourseId && l.Order == lesson.Order + 1 && !l.IsDeleted);

        if (nextLesson != null)
        {
            nextLesson.Order--;
            nextLesson.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Lessons.Update(nextLesson);
        }

        lesson.Order++;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        lesson.Course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(lesson.Course);
        _unitOfWork.Lessons.Update(lesson);

        await _unitOfWork.CompleteAsync();
    }
}