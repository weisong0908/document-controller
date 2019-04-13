using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentController.WebAPI.Migrations
{
    public partial class AddedIsRemovedtoDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "is_removed",
                table: "documents",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "document_versions",
                keyColumn: "id",
                keyValue: 1,
                column: "effective_date",
                value: new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "document_versions",
                keyColumn: "id",
                keyValue: 2,
                column: "effective_date",
                value: new DateTime(2019, 4, 13, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_removed",
                table: "documents");

            migrationBuilder.UpdateData(
                table: "document_versions",
                keyColumn: "id",
                keyValue: 1,
                column: "effective_date",
                value: new DateTime(2018, 11, 29, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "document_versions",
                keyColumn: "id",
                keyValue: 2,
                column: "effective_date",
                value: new DateTime(2018, 12, 9, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
