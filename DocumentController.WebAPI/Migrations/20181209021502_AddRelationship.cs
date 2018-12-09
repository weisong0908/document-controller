using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentController.WebAPI.Migrations
{
    public partial class AddRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_document_versions_document_id",
                table: "document_versions",
                column: "document_id");

            migrationBuilder.AddForeignKey(
                name: "FK_document_versions_documents_document_id",
                table: "document_versions",
                column: "document_id",
                principalTable: "documents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_document_versions_documents_document_id",
                table: "document_versions");

            migrationBuilder.DropIndex(
                name: "IX_document_versions_document_id",
                table: "document_versions");
        }
    }
}
