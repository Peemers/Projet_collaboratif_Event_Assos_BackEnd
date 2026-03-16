using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAssos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Evenements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Lieu = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    NbMin = table.Column<int>(type: "int", nullable: false),
                    NbMax = table.Column<int>(type: "int", nullable: false),
                    StatutEvenement = table.Column<int>(type: "int", nullable: false),
                    ListeAttenteActive = table.Column<bool>(type: "bit", nullable: false),
                    DateLimiteInscription = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateDeCreation = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    DateMaj = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evenements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Pseudo = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvenementCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    EvenementsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvenementCategories", x => new { x.CategoriesId, x.EvenementsId });
                    table.ForeignKey(
                        name: "FK_EvenementCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvenementCategories_Evenements_EvenementsId",
                        column: x => x.EvenementsId,
                        principalTable: "Evenements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    MembreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvenementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstEnAttente = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => new { x.MembreId, x.EvenementId });
                    table.ForeignKey(
                        name: "FK_Inscriptions_Evenements_EvenementId",
                        column: x => x.EvenementId,
                        principalTable: "Evenements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Membres_MembreId",
                        column: x => x.MembreId,
                        principalTable: "Membres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvenementCategories_EvenementsId",
                table: "EvenementCategories",
                column: "EvenementsId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_EvenementId",
                table: "Inscriptions",
                column: "EvenementId");

            migrationBuilder.CreateIndex(
                name: "IX_Membres_Email",
                table: "Membres",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membres_Pseudo",
                table: "Membres",
                column: "Pseudo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvenementCategories");

            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Evenements");

            migrationBuilder.DropTable(
                name: "Membres");
        }
    }
}
