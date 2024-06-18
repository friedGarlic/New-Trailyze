using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changestolistmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkingDays",
                table: "ListName2",
                newName: "WorkingDaysStart");

            migrationBuilder.RenameColumn(
                name: "TimeInAndOut",
                table: "ListName2",
                newName: "WorkingDaysEnds");

            migrationBuilder.RenameColumn(
                name: "WorkingDays",
                table: "ListName",
                newName: "WorkingDaysStart");

            migrationBuilder.RenameColumn(
                name: "TimeInAndOut",
                table: "ListName",
                newName: "WorkingDaysEnds");

            migrationBuilder.AddColumn<string>(
                name: "TimeIn",
                table: "ListName2",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeOut",
                table: "ListName2",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeIn",
                table: "ListName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeOut",
                table: "ListName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeIn",
                table: "ListName2");

            migrationBuilder.DropColumn(
                name: "TimeOut",
                table: "ListName2");

            migrationBuilder.DropColumn(
                name: "TimeIn",
                table: "ListName");

            migrationBuilder.DropColumn(
                name: "TimeOut",
                table: "ListName");

            migrationBuilder.RenameColumn(
                name: "WorkingDaysStart",
                table: "ListName2",
                newName: "WorkingDays");

            migrationBuilder.RenameColumn(
                name: "WorkingDaysEnds",
                table: "ListName2",
                newName: "TimeInAndOut");

            migrationBuilder.RenameColumn(
                name: "WorkingDaysStart",
                table: "ListName",
                newName: "WorkingDays");

            migrationBuilder.RenameColumn(
                name: "WorkingDaysEnds",
                table: "ListName",
                newName: "TimeInAndOut");
        }
    }
}
