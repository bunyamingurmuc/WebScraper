using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScraperAPI.Migrations
{
    public partial class deneme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filter_Categories_CategoryId",
                table: "Filter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filter",
                table: "Filter");

            migrationBuilder.RenameTable(
                name: "Filter",
                newName: "Filters");

            migrationBuilder.RenameIndex(
                name: "IX_Filter_CategoryId",
                table: "Filters",
                newName: "IX_Filters_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filters",
                table: "Filters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_Categories_CategoryId",
                table: "Filters",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filters_Categories_CategoryId",
                table: "Filters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filters",
                table: "Filters");

            migrationBuilder.RenameTable(
                name: "Filters",
                newName: "Filter");

            migrationBuilder.RenameIndex(
                name: "IX_Filters_CategoryId",
                table: "Filter",
                newName: "IX_Filter_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filter",
                table: "Filter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Filter_Categories_CategoryId",
                table: "Filter",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
