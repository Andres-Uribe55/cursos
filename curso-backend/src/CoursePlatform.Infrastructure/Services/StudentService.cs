using System.Text;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Interfaces;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Services;

public class StudentService : IStudentService
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;

    public StudentService(
        UserManager<User> userManager,
        IEmailService emailService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _emailService = emailService;
        _context = context;
    }

    public async Task<PaginatedResponse<StudentDto>> SearchAsync(string? q, int page = 1, int pageSize = 10)
    {
        var query = _userManager.Users
            .Where(u => _context.UserRoles
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .Any(x => x.UserId == u.Id && x.Name == AppRoles.Student))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(u => 
                u.FirstName.Contains(q) || 
                u.LastName.Contains(q) || 
                u.Email!.Contains(q) || 
                (u.DocumentNumber != null && u.DocumentNumber.Contains(q)));
        }

        var totalItems = await query.CountAsync();
        var students = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new StudentDto(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email!,
                u.DocumentNumber,
                u.Age,
                u.CreatedAt
            ))
            .ToListAsync();

        return new PaginatedResponse<StudentDto>(students, totalItems, page, pageSize);
    }

    public async Task<StudentDto> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null || !await _userManager.IsInRoleAsync(user, AppRoles.Student))
            throw new KeyNotFoundException("Student not found");

        return new StudentDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email!,
            user.DocumentNumber,
            user.Age,
            user.CreatedAt
        );
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"Email {dto.Email} is already taken");

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DocumentNumber = dto.DocumentNumber,
            Age = dto.Age,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true // Auto-confirm for manually created students
        };

        var password = GenerateRandomPassword();
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, AppRoles.Student);

        // Send email
        var subject = "Bienvenido a la Plataforma de Cursos";
        var body = $@"
            <h1>Hola {user.FirstName},</h1>
            <p>Se ha creado tu cuenta de estudiante.</p>
            <p><strong>Usuario:</strong> {user.Email}</p>
            <p><strong>Contraseña:</strong> {password}</p>
            <p>Por favor inicia sesión y cambia tu contraseña.</p>
            <br>
            <a href='http://localhost:4200/login'>Ir a la plataforma</a>
        ";

        try
        {
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch (Exception ex)
        {
            // Log error but don't fail creation? Or fail? 
            // Ideally we should log it. For now let's print to console or ignore strict requirement to fail.
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }

        return new StudentDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.DocumentNumber,
            user.Age,
            user.CreatedAt
        );
    }

    public async Task ImportCsvAsync(Stream fileStream)
    {
        using var stream = new StreamReader(fileStream);
        var lineCount = 0;
        
        while (!stream.EndOfStream)
        {
            var line = await stream.ReadLineAsync();
            lineCount++;
            
            if (string.IsNullOrWhiteSpace(line) || lineCount == 1) continue; // Skip header or empty

            var parts = line.Split(',');
            if (parts.Length < 5) continue;

            // Format: nombres,apellidos,documento,edad,email
            var firstName = parts[0].Trim();
            var lastName = parts[1].Trim();
            var document = parts[2].Trim();
            
            // Try parse age
            int? age = int.TryParse(parts[3].Trim(), out var a) ? a : null;
            
            var email = parts[4].Trim();

            try
            {
                await CreateAsync(new CreateStudentDto(
                    firstName, 
                    lastName, 
                    email, 
                    document, 
                    age
                ));
            }
            catch
            {
                // Continue with next student if one fails
                // In production might want to return report of failures
            }
        }
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null || !await _userManager.IsInRoleAsync(user, AppRoles.Student))
            throw new KeyNotFoundException("Student not found");

        await _userManager.DeleteAsync(user);
    }

    private string GenerateRandomPassword()
    {
        // Simple random password that meets complexity
        // 8 chars, 1 uppercase, 1 lowercase, 1 digit, 1 special
        return Guid.NewGuid().ToString("N").Substring(0, 8) + "Aa1!"; 
    }
}
