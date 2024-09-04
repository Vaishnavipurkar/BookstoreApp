using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookstoreApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPaidToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "purchases",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "purchases");
        }
    }
}
