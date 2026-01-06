namespace CoursePlatform.Application.DTOs;

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword
);

public record LoginDto(
    string Email,
    string Password
);

public record AuthResponseDto(
    string Token,
    string RefreshToken,
    string Email,
    string Role
);

public record RefreshTokenRequestDto(
    string Token,
    string RefreshToken
);