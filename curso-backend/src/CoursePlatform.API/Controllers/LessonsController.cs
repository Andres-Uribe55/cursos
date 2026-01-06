using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<List<LessonDto>>> GetByCourse(Guid courseId)
    {
        var lessons = await _lessonService.GetByCourseIdAsync(courseId);
        return Ok(lessons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LessonDto>> GetById(Guid id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        return Ok(lesson);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<LessonDto>> Create(CreateLessonDto dto)
    {
        var lesson = await _lessonService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, lesson);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id}")]
    public async Task<ActionResult<LessonDto>> Update(Guid id, UpdateLessonDto dto)
    {
        var lesson = await _lessonService.UpdateAsync(id, dto);
        return Ok(lesson);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lessonService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch("{id}/move-up")]
    public async Task<IActionResult> MoveUp(Guid id)
    {
        await _lessonService.MoveUpAsync(id);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch("{id}/move-down")]
    public async Task<IActionResult> MoveDown(Guid id)
    {
        await _lessonService.MoveDownAsync(id);
        return NoContent();
    }
}