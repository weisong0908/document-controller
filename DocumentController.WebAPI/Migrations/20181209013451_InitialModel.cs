using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DocumentController.WebAPI.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DocumentNumber = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Function = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DocumentId = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<string>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    Progress = table.Column<string>(nullable: true),
                    DescriptionOfChange = table.Column<string>(nullable: true),
                    PurposeOfChange = table.Column<string>(nullable: true),
                    Requestor = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Location_PDF = table.Column<string>(nullable: true),
                    Location_Editable = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVersions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentVersions");
        }
    }
}
