using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addPropertyToAccountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HoursRequired",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursRequired",
                table: "AspNetUsers");
        }
    }
}
