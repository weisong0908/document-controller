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
                name: "document_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    document_id = table.Column<int>(nullable: false),
                    version_number = table.Column<string>(nullable: true),
                    effective_date = table.Column<DateTime>(nullable: true),
                    progress = table.Column<string>(nullable: true),
                    description_of_change = table.Column<string>(nullable: true),
                    purpose_of_change = table.Column<string>(nullable: true),
                    requestor = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    location_pdf = table.Column<string>(nullable: true),
                    location_editable = table.Column<string>(nullable: true),
                    is_removed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    document_number = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    department = table.Column<string>(nullable: true),
                    function = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document_versions");

            migrationBuilder.DropTable(
                name: "documents");
        }
    }
}
