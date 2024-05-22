using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changesInAccountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursCompleted",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "HoursRemaining",
                table: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HoursCompleted",
                table: "Logs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HoursRemaining",
                table: "Logs",
                type: "float",
                nullable: true);
        }
    }
}
