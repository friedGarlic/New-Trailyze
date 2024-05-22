using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class testforaddinglistpropertyacc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Account_ModelId",
                table: "RequirementFile",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequirementFile_Account_ModelId",
                table: "RequirementFile",
                column: "Account_ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequirementFile_AspNetUsers_Account_ModelId",
                table: "RequirementFile",
                column: "Account_ModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequirementFile_AspNetUsers_Account_ModelId",
                table: "RequirementFile");

            migrationBuilder.DropIndex(
                name: "IX_RequirementFile_Account_ModelId",
                table: "RequirementFile");

            migrationBuilder.DropColumn(
                name: "Account_ModelId",
                table: "RequirementFile");
        }
    }
}
