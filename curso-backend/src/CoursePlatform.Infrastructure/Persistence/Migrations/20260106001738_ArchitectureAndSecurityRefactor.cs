using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoursePlatform.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ArchitectureAndSecurityRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Used = table.Column<bool>(type: "boolean", nullable: false),
                    Invalidated = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "SecurityStamp", "UserName" },
                values: new object[] { "STATIC-CONCURRENCY-STAMP-ADMIN", "admin@example.com", "Super", "Admin", "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "STATIC-SECURITY-STAMP-ADMIN", "admin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "ConcurrencyStamp", "CreatedAt", "DocumentNumber", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2", 0, 25, "STATIC-CONCURRENCY-STAMP-STUDENT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "student@example.com", true, "John", "Student", false, null, "STUDENT@EXAMPLE.COM", "STUDENT@EXAMPLE.COM", "AQAAAAIAAYagAAAAEPFWPsgT+Wt6asbbjl1YhRUnm4ge4CDpgk5xBBl4kX7nLpgxPeuIk8VFKxrgzGXf2w==", null, false, "STATIC-SECURITY-STAMP-STUDENT", false, "student@example.com" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "Description", "IsDeleted", "ResourceUrl", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Curso completo de Angular con señales y componentes standalone.", false, "", "Published", "Angular Avanzado", new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc), "Aprende Clean Architecture y patrones de diseño.", false, "", "Draft", "ASP.NET Core Architecture", new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2", "2" });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "CourseId", "CreatedAt", "IsDeleted", "Order", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1a94e0f3-0e51-4a1d-9f84-a68cb6a108e8"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6350), false, 1, "Estructura de Carpetas", new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6350) },
                    { new Guid("1e7e68c5-7b85-4867-8d17-9daba062c70d"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6345), false, 3, "Servicios", new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6345) },
                    { new Guid("44f311b7-968a-4a93-813e-8abb1813042a"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6339), false, 2, "Componentes", new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6340) },
                    { new Guid("46ec3c0a-2db0-4163-88c6-1fa9225444de"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6333), false, 1, "Introducción", new DateTime(2026, 1, 6, 0, 17, 38, 125, DateTimeKind.Utc).AddTicks(6333) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Status",
                table: "Courses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Title",
                table: "Courses",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Status",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Title",
                table: "Courses");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "2" });

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("1a94e0f3-0e51-4a1d-9f84-a68cb6a108e8"));

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("1e7e68c5-7b85-4867-8d17-9daba062c70d"));

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("44f311b7-968a-4a93-813e-8abb1813042a"));

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "Id",
                keyValue: new Guid("46ec3c0a-2db0-4163-88c6-1fa9225444de"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "SecurityStamp", "UserName" },
                values: new object[] { "STATIC-CONCURRENCY-STAMP-FOR-SEED", "test@example.com", "Admin", "User", "TEST@EXAMPLE.COM", "TEST@EXAMPLE.COM", "STATIC-SECURITY-STAMP-FOR-SEED", "test@example.com" });
        }
    }
}
