using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addpropertytoojtlocationlists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeInAndOut",
                table: "ListName2",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDays",
                table: "ListName2",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeInAndOut",
                table: "ListName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkingDays",
                table: "ListName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeInAndOut",
                table: "ListName2");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "ListName2");

            migrationBuilder.DropColumn(
                name: "TimeInAndOut",
                table: "ListName");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "ListName");
        }
    }
}
