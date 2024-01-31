using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApp.Migrations
{
    /// <inheritdoc />
    public partial class TransactionMigration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ammount = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AspNetUsers_UserId",
                table: "Assets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AspNetUsers_UserId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "Transactions");

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
    }
}
