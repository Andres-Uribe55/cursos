using CoursePlatform.Application.DTOs;


namespace CoursePlatform.Application.Interfaces;

public interface IStudentService
{
    Task<PaginatedResponse<StudentDto>> SearchAsync(string? q, int page = 1, int pageSize = 10);
    Task<StudentDto> GetByIdAsync(string id);
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task ImportCsvAsync(Stream fileStream);
    Task DeleteAsync(string id);
}
