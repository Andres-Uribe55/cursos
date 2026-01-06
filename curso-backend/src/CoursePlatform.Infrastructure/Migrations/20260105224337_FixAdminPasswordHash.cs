using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "STATIC-CONCURRENCY-STAMP-FOR-SEED", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEPFWPsgT+Wt6asbbjl1YhRUnm4ge4CDpgk5xBBl4kX7nLpgxPeuIk8VFKxrgzGXf2w==", "STATIC-SECURITY-STAMP-FOR-SEED" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e8d5cb3-5d5b-4841-b65e-7215a8822e52", new DateTime(2026, 1, 5, 22, 14, 50, 828, DateTimeKind.Utc).AddTicks(4794), "AQAAAAIAAYagAAAAELqKzRdvqmxZ3dVf0TYLqzrqvmGxqYDxzKqT0qxzKqT0qxzKqT0qxzKqT0qxzKqT0==", "eded98bd-03a2-4a2a-bc0f-07b74a352cdb" });
        }
    }
}
