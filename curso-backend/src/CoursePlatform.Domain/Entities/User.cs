using Microsoft.AspNetCore.Identity;

namespace CoursePlatform.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public int? Age { get; set; }
    public DateTime CreatedAt { get; set; }
}