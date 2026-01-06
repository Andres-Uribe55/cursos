using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<PaginatedResponse<CourseDto>>> Search(
        [FromQuery] string? q,
        [FromQuery] CourseStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Enforce published only for non-admins
        if (!User.IsInRole(AppRoles.Admin))
        {
            status = CourseStatus.Published;
        }

        var searchParams = new CourseSearchParams(q, status, page, pageSize);
        var result = await _courseService.SearchAsync(searchParams);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDto>> GetById(Guid id)
    {
        var course = await _courseService.GetByIdAsync(id);
        return Ok(course);
    }

    [HttpGet("{id}/summary")]
    public async Task<ActionResult<CourseSummaryDto>> GetSummary(Guid id)
    {
        var summary = await _courseService.GetSummaryAsync(id);
        return Ok(summary);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<CourseDto>> Create(CreateCourseDto dto)
    {
        var course = await _courseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id}")]
    public async Task<ActionResult<CourseDto>> Update(Guid id, UpdateCourseDto dto)
    {
        var course = await _courseService.UpdateAsync(id, dto);
        return Ok(course);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _courseService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _courseService.PublishAsync(id);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch("{id}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        await _courseService.UnpublishAsync(id);
        return NoContent();
    }
}