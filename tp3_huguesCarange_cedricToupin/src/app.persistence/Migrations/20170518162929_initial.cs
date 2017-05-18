using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace app.persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artistes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    NAS = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: false),
                    NomDeScene = table.Column<string>(nullable: true),
                    Prenom = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artistes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Nom = table.Column<string>(nullable: false),
                    Prenom = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groupes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Cachet = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contrats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Cachet = table.Column<int>(nullable: false),
                    DateContrat = table.Column<DateTime>(nullable: false),
                    DatePresentation = table.Column<DateTime>(nullable: false),
                    Depot = table.Column<int>(nullable: false),
                    HeureDebut = table.Column<DateTime>(nullable: false),
                    HeureFin = table.Column<DateTime>(nullable: false),
                    IdClient = table.Column<int>(nullable: false),
                    IdGroupe = table.Column<int>(nullable: false),
                    MontantFinal = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contrats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contrats_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contrats_Groupes_IdGroupe",
                        column: x => x.IdGroupe,
                        principalTable: "Groupes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Membres",
                columns: table => new
                {
                    IdArtiste = table.Column<int>(nullable: false),
                    IdGroupe = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membres", x => new { x.IdArtiste, x.IdGroupe });
                    table.ForeignKey(
                        name: "FK_Membres_Artistes_IdArtiste",
                        column: x => x.IdArtiste,
                        principalTable: "Artistes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Membres_Groupes_IdGroupe",
                        column: x => x.IdGroupe,
                        principalTable: "Groupes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    DatePaiement = table.Column<DateTime>(nullable: false),
                    DateProduction = table.Column<DateTime>(nullable: false),
                    IdContrat = table.Column<int>(nullable: false),
                    ModePaiement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factures_Contrats_IdContrat",
                        column: x => x.IdContrat,
                        principalTable: "Contrats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artistes_NAS",
                table: "Artistes",
                column: "NAS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contrats_IdClient",
                table: "Contrats",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Contrats_IdGroupe",
                table: "Contrats",
                column: "IdGroupe");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_IdContrat",
                table: "Factures",
                column: "IdContrat");

            migrationBuilder.CreateIndex(
                name: "IX_Groupes_Nom",
                table: "Groupes",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membres_IdArtiste",
                table: "Membres",
                column: "IdArtiste");

            migrationBuilder.CreateIndex(
                name: "IX_Membres_IdGroupe",
                table: "Membres",
                column: "IdGroupe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factures");

            migrationBuilder.DropTable(
                name: "Membres");

            migrationBuilder.DropTable(
                name: "Contrats");

            migrationBuilder.DropTable(
                name: "Artistes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Groupes");
        }
    }
}
