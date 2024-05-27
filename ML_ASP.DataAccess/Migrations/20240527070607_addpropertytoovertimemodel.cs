using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addpropertytoovertimemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "Overtime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Overtime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Overtime");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Overtime");
        }
    }
}
