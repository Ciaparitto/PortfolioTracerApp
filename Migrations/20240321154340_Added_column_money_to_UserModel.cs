using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_column_money_to_UserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "AspNetUsers");
        }
    }
}
