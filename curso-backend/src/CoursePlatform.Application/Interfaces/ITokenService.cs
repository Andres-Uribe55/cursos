using CoursePlatform.Application.DTOs;
using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Application.Interfaces;

public interface ITokenService
{
    Task<AuthResponseDto> GenerateAuthResponseAsync(User user);
    Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);
}
