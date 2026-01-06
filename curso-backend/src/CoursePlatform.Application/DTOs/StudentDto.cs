using System.ComponentModel.DataAnnotations;


namespace CoursePlatform.Application.DTOs;

public record StudentDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? DocumentNumber,
    int? Age,
    DateTime CreatedAt
);

public record CreateStudentDto(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] [EmailAddress] string Email,
    string? DocumentNumber,
    int? Age
);


