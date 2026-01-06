using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResponse<CourseDto>> SearchAsync(CourseSearchParams searchParams)
    {
        var (items, totalCount) = await _unitOfWork.Courses.SearchAsync(
            searchParams.Q, 
            searchParams.Status, 
            searchParams.Page, 
            searchParams.PageSize);

        var courses = items.Select(c => new CourseDto(
            c.Id,
            c.Title,
            c.Description,
            c.ResourceUrl,
            c.Status,
            c.IsDeleted,
            c.CreatedAt,
            c.UpdatedAt
        )).ToList();

        return new PaginatedResponse<CourseDto>(courses, totalCount, searchParams.Page, searchParams.PageSize);
    }

    public async Task<CourseDto> GetByIdAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        return new CourseDto(
            course.Id,
            course.Title,
            course.Description,
            course.ResourceUrl,
            course.Status,
            course.IsDeleted,
            course.CreatedAt,
            course.UpdatedAt
        );
    }

    public async Task<CourseSummaryDto> GetSummaryAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetFirstOrDefaultAsync(c => c.Id == id, "Lessons");

        if (course == null)
            throw new CourseNotFoundException(id);

        var totalLessons = course.Lessons.Count(l => !l.IsDeleted);

        return new CourseSummaryDto(
            course.Id,
            course.Title,
            course.Description,
            course.ResourceUrl,
            course.Status,
            totalLessons,
            course.UpdatedAt
        );
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            ResourceUrl = dto.ResourceUrl ?? string.Empty,
            Status = CourseStatus.Draft,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };



        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.CompleteAsync();

        return new CourseDto(
            course.Id,
            course.Title,
            course.Description,
            course.ResourceUrl,
            course.Status,
            course.IsDeleted,
            course.CreatedAt,
            course.UpdatedAt
        );
    }

    public async Task<CourseDto> UpdateAsync(Guid id, UpdateCourseDto dto)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        course.Title = dto.Title;
        if (dto.Description != null)
            course.Description = dto.Description;
        if (dto.ResourceUrl != null)
            course.ResourceUrl = dto.ResourceUrl;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CompleteAsync();

        return new CourseDto(
            course.Id,
            course.Title,
            course.Description,
            course.ResourceUrl,
            course.Status,
            course.IsDeleted,
            course.CreatedAt,
            course.UpdatedAt
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CompleteAsync();
    }

    public async Task PublishAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetFirstOrDefaultAsync(c => c.Id == id, "Lessons");

        if (course == null)
            throw new CourseNotFoundException(id);

        var hasActiveLessons = course.Lessons.Any(l => !l.IsDeleted);
        if (!hasActiveLessons)
            throw new CannotPublishCourseException();

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CompleteAsync();
    }

    public async Task UnpublishAsync(Guid id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.CompleteAsync();
    }
}