using System.ComponentModel.DataAnnotations;

namespace CoursePlatform.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty; // Jti of the access token
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Used { get; set; }
    public bool Invalidated { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Navigation property
    public User User { get; set; }
}
