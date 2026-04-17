using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAssos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifEntitéEtConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inscriptions",
                table: "Inscriptions");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Inscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inscriptions",
                table: "Inscriptions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_MembreId_EvenementId",
                table: "Inscriptions",
                columns: new[] { "MembreId", "EvenementId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Inscriptions",
                table: "Inscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Inscriptions_MembreId_EvenementId",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Inscriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inscriptions",
                table: "Inscriptions",
                columns: new[] { "MembreId", "EvenementId" });
        }
    }
}
