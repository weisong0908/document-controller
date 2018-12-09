using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentController.WebAPI.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "documents",
                columns: new[] { "id", "department", "document_number", "function", "location", "status", "title", "type" },
                values: new object[] { 1, null, "DOC-01", null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "document_versions",
                columns: new[] { "id", "description_of_change", "document_id", "effective_date", "is_removed", "location_editable", "location_pdf", "progress", "purpose_of_change", "remarks", "requestor", "version_number" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2018, 11, 29, 0, 0, 0, 0, DateTimeKind.Local), null, null, null, null, null, null, null, "1" },
                    { 2, null, 1, new DateTime(2018, 12, 9, 0, 0, 0, 0, DateTimeKind.Local), null, null, null, null, null, null, null, "2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "document_versions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "document_versions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "documents",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
