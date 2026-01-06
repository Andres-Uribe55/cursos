using System.ComponentModel.DataAnnotations;

namespace CoursePlatform.Application.DTOs;

public record UserProfileDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? DocumentNumber,
    int? Age,
    string Role
);

public class UpdateProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public int? Age { get; set; }
}

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
