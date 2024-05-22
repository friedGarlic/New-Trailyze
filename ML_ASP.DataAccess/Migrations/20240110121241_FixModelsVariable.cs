using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixModelsVariable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskCompleted",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "WeeklyReport",
                table: "Submissions");

            migrationBuilder.AddColumn<int>(
                name: "WeeklyReportRemaining",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyReportRemaining",
                table: "AspNetUsers");

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
        }
    }
}
