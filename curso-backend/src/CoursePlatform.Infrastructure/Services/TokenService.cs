using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CoursePlatform.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public TokenService(IConfiguration configuration, UserManager<User> userManager, ApplicationDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthResponseDto> GenerateAuthResponseAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (token, jti) = GenerateJwtToken(user, roles);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            JwtId = jti,
            UserId = user.Id,
            Token = Guid.NewGuid().ToString(),
            CreationDate = DateTime.UtcNow, // Use UtcNow
            ExpiryDate = DateTime.UtcNow.AddMonths(1),
            Used = false,
            Invalidated = false
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return new AuthResponseDto(token, refreshToken.Token, user.Email!, roles.FirstOrDefault() ?? "Student");
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromExpiredToken(token);
        if (validatedToken == null)
        {
            throw new Exception("Invalid Token");
        }

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == refreshToken);

        if (storedRefreshToken == null)
            throw new Exception("Refresh token does not exist");

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            throw new Exception("Refresh token has expired");

        if (storedRefreshToken.Invalidated)
            throw new Exception("Refresh token has been invalidated");

        if (storedRefreshToken.Used)
            throw new Exception("Refresh token has been used");

        if (storedRefreshToken.JwtId != jti)
            throw new Exception("Refresh token does not match this JWT");

        storedRefreshToken.Used = true;
        _context.RefreshTokens.Update(storedRefreshToken);
        await _context.SaveChangesAsync();

        var userId = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null) throw new Exception("User not found");

        return await GenerateAuthResponseAsync(user);
    }

    private (string Token, string Jti) GenerateJwtToken(User user, IList<string> roles)
    {
        var jti = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Short lived access token
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), jti);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false, // flexible for refresh
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "")),
            ValidateLifetime = false // Validation logic handles expiration
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken || 
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}