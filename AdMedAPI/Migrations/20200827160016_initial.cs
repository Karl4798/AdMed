using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdMedAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrimaryContact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Relationship = table.Column<string>(nullable: false),
                    PhysicalAddress = table.Column<string>(nullable: false),
                    PostalAddress = table.Column<string>(nullable: false),
                    PostCode = table.Column<string>(nullable: false),
                    IdentityNumber = table.Column<string>(nullable: false),
                    HomeTelephoneNumber = table.Column<string>(nullable: true),
                    WorkTelephoneNumber = table.Column<string>(nullable: true),
                    CellTelephoneNumber = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    GenderString = table.Column<string>(nullable: true),
                    Allergies = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    IdentityNumber = table.Column<string>(nullable: false),
                    MedicalAid = table.Column<string>(nullable: false),
                    MedicalAidNumber = table.Column<string>(nullable: true),
                    DoctorName = table.Column<string>(nullable: true),
                    HomeTelephoneNumber = table.Column<string>(nullable: true),
                    WorkTelephoneNumber = table.Column<string>(nullable: true),
                    CellTelephoneNumber = table.Column<string>(nullable: false),
                    Undertaker = table.Column<string>(nullable: false),
                    UndertakerTelephoneNumber = table.Column<string>(nullable: false),
                    PharmacyName = table.Column<string>(nullable: false),
                    PharmacyTelephoneNumber = table.Column<string>(nullable: false),
                    PharmacyFaxNumber = table.Column<string>(nullable: false),
                    PrimaryContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_PrimaryContact_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "PrimaryContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Relationship = table.Column<string>(nullable: false),
                    PhysicalAddress = table.Column<string>(nullable: false),
                    PostalAddress = table.Column<string>(nullable: false),
                    PostCode = table.Column<string>(nullable: false),
                    IdentityNumber = table.Column<string>(nullable: false),
                    HomeTelephoneNumber = table.Column<string>(nullable: true),
                    WorkTelephoneNumber = table.Column<string>(nullable: true),
                    CellTelephoneNumber = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyContacts_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_PrimaryContactId",
                table: "Applications",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_ApplicationId",
                table: "EmergencyContacts",
                column: "ApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmergencyContacts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "PrimaryContact");
        }
    }
}
