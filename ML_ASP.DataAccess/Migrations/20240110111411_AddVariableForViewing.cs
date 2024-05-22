using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVariableForViewing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskCompleted",
                table: "Submissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeeklyReport",
                table: "Submissions",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskCompleted",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "WeeklyReport",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "HoursCompleted",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "HoursRemaining",
                table: "Logs");
        }
    }
}
