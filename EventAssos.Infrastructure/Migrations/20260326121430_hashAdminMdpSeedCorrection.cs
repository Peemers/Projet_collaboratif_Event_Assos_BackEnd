using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAssos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class hashAdminMdpSeedCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Membres",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "DateInscription", "Password" },
                values: new object[] { new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$MUHgZCCeSF4CY13n9kA4EeMatlqokJqxwp8gEUZGd8NEPBkVqhKJ2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Membres",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "DateInscription", "Password" },
                values: new object[] { new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$tVLctcyGxgvywS2RPlOskOdfHTYFjlLi2qYcp/IlILBUU0USZKNJq" });
        }
    }
}
