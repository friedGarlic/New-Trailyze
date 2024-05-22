using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addFileAdjustmentProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_ApplicationForm",
                table: "RequirementFile");

            migrationBuilder.DropColumn(
                name: "File_ContractAgreement",
                table: "RequirementFile");

            migrationBuilder.RenameColumn(
                name: "File_TimeSheet",
                table: "RequirementFile",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "File_ProgressReport",
                table: "RequirementFile",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "File_ParentalConsent",
                table: "RequirementFile",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "RequirementFile",
                newName: "File_TimeSheet");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "RequirementFile",
                newName: "File_ProgressReport");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "RequirementFile",
                newName: "File_ParentalConsent");

            migrationBuilder.AddColumn<string>(
                name: "File_ApplicationForm",
                table: "RequirementFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "File_ContractAgreement",
                table: "RequirementFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
