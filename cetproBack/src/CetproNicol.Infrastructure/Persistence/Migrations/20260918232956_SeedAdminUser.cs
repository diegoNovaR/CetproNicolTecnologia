using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CetproNicol.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "Id", "Apellido", "Email", "FechaRegistro", "Nombre", "PasswordHash", "Rol", "Telefono" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "CetproNicol", "admin@cetpronicol.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", "$2a$11$h0/WqTJnQFJNVvDj9ofT5eQt4JSwlI2ZG1tm2MtcUOU.ghOpnNOeK", "admin", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
