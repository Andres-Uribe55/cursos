using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoursePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                INSERT INTO ""AspNetRoles"" (""Id"", ""ConcurrencyStamp"", ""Name"", ""NormalizedName"")
                VALUES ('1', NULL, 'Admin', 'ADMIN')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""AspNetRoles"" (""Id"", ""ConcurrencyStamp"", ""Name"", ""NormalizedName"")
                VALUES ('2', NULL, 'Student', 'STUDENT')
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""AspNetUsers"" (""Id"", ""AccessFailedCount"", ""ConcurrencyStamp"", ""CreatedAt"", ""Email"", ""EmailConfirmed"", ""LockoutEnabled"", ""LockoutEnd"", ""NormalizedEmail"", ""NormalizedUserName"", ""PasswordHash"", ""PhoneNumber"", ""PhoneNumberConfirmed"", ""SecurityStamp"", ""TwoFactorEnabled"", ""UserName"", ""FirstName"", ""LastName"", ""Age"", ""DocumentNumber"")
                VALUES ('1', 0, '1e8d5cb3-5d5b-4841-b65e-7215a8822e52', NOW(), 'test@example.com', true, false, NULL, 'TEST@EXAMPLE.COM', 'TEST@EXAMPLE.COM', 'AQAAAAIAAYagAAAAELqKzRdvqmxZ3dVf0TYLqzrqvmGxqYDxzKqT0qxzKqT0qxzKqT0qxzKqT0xzKqT0==', NULL, false, 'eded98bd-03a2-4a2a-bc0f-07b74a352cdb', false, 'test@example.com', 'Admin', 'User', NULL, NULL)
                ON CONFLICT (""Id"") DO UPDATE 
                SET ""FirstName"" = 'Admin', ""LastName"" = 'User';
            ");

            migrationBuilder.Sql(@"
                INSERT INTO ""AspNetUserRoles"" (""RoleId"", ""UserId"")
                VALUES ('1', '1')
                ON CONFLICT (""UserId"", ""RoleId"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "b3deec69-6795-4bc0-88d1-7c0e2cb360ee", new DateTime(2026, 1, 5, 20, 20, 46, 753, DateTimeKind.Utc).AddTicks(4774), "3528dbf6-1950-46b8-95bc-1b2985588b31" });
        }
    }
}
