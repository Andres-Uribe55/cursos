using CoursePlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoursePlatform.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Course configuration
        builder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasIndex(e => e.Title); // Index for search
            entity.HasIndex(e => e.Status); // Index for filtering
            entity.HasQueryFilter(e => !e.IsDeleted);
            
            entity.HasMany(e => e.Lessons)
                  .WithOne(e => e.Course)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Lesson configuration
        builder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.HasQueryFilter(e => !e.IsDeleted);
            
            // PostgreSQL syntax for filtered index might differ, handled by EF Core provider usually
            entity.HasIndex(e => new { e.CourseId, e.Order })
                  .IsUnique()
                  .HasFilter("\"IsDeleted\" = false");
        });

        // RefreshToken configuration
        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired();
            entity.Property(e => e.JwtId).IsRequired();
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data expandido
        SeedData(builder);
    }

    private void SeedData(ModelBuilder builder)
    {
        // ... (Keep existing Admin and Roles) ...
        // Seed Roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper() },
            new IdentityRole { Id = "2", Name = AppRoles.Student, NormalizedName = AppRoles.Student.ToUpper() }
        );

        // Admin User
        var adminUser = new User
        {
            Id = "1",
            UserName = "admin@example.com",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEPFWPsgT+Wt6asbbjl1YhRUnm4ge4CDpgk5xBBl4kX7nLpgxPeuIk8VFKxrgzGXf2w==", // Test123!
            SecurityStamp = "STATIC-SECURITY-STAMP-ADMIN",
            ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-ADMIN",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            FirstName = "Super",
            LastName = "Admin"
        };
        
        // Student User
        var studentUser = new User
        {
            Id = "2",
            UserName = "student@example.com",
            NormalizedUserName = "STUDENT@EXAMPLE.COM",
            Email = "student@example.com",
            NormalizedEmail = "STUDENT@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEPFWPsgT+Wt6asbbjl1YhRUnm4ge4CDpgk5xBBl4kX7nLpgxPeuIk8VFKxrgzGXf2w==", // Test123!
            SecurityStamp = "STATIC-SECURITY-STAMP-STUDENT",
            ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-STUDENT",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            FirstName = "John",
            LastName = "Student",
            Age = 25
        };

        builder.Entity<User>().HasData(adminUser, studentUser);

        // Assign Roles
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = "1", UserId = "1" },
            new IdentityUserRole<string> { RoleId = "2", UserId = "2" }
        );

        // Seed Courses
        var courseId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var courseId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.Entity<Course>().HasData(
            new Course
            {
                Id = courseId1,
                Title = "Angular Avanzado",
                Description = "Curso completo de Angular con señales y componentes standalone.",
                Status = CourseStatus.Published,
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new Course
            {
                Id = courseId2,
                Title = "ASP.NET Core Architecture",
                Description = "Aprende Clean Architecture y patrones de diseño.",
                Status = CourseStatus.Draft,
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 1, 2, 10, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 1, 2, 10, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed Lessons
        builder.Entity<Lesson>().HasData(
            new Lesson { Id = Guid.NewGuid(), CourseId = courseId1, Title = "Introducción", Order = 1, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Lesson { Id = Guid.NewGuid(), CourseId = courseId1, Title = "Componentes", Order = 2, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Lesson { Id = Guid.NewGuid(), CourseId = courseId1, Title = "Servicios", Order = 3, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Lesson { Id = Guid.NewGuid(), CourseId = courseId2, Title = "Estructura de Carpetas", Order = 1, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );
    }
}