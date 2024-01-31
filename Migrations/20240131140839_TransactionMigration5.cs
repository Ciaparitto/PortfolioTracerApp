using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApp.Migrations
{
    /// <inheritdoc />
    public partial class TransactionMigration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AspNetUsers_UserId",
                table: "Assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assets",
                table: "Assets");

            migrationBuilder.RenameTable(
                name: "Assets",
                newName: "AssetModel");

            migrationBuilder.RenameIndex(
                name: "IX_Assets_UserId",
                table: "AssetModel",
                newName: "IX_AssetModel_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetModel",
                table: "AssetModel",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetModel_AspNetUsers_UserId",
                table: "AssetModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetModel_AspNetUsers_UserId",
                table: "AssetModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetModel",
                table: "AssetModel");

            migrationBuilder.RenameTable(
                name: "AssetModel",
                newName: "Assets");

            migrationBuilder.RenameIndex(
                name: "IX_AssetModel_UserId",
                table: "Assets",
                newName: "IX_Assets_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assets",
                table: "Assets",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AspNetUsers_UserId",
                table: "Assets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
