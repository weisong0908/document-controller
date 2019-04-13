using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentController.WebAPI.Migrations
{
    public partial class ChangedeffectiveDatetodatetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "effective_date",
                table: "document_versions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "effective_date",
                table: "document_versions",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }
    }
}
