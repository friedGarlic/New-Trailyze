using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ML_ASP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class accmodelDataAdds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSpanDuration",
                table: "AspNetUsers",
                newName: "TotalTime");

            migrationBuilder.AlterColumn<int>(
                name: "HoursRemaining",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HoursCompleted",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinutesCompleted",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinutesRemaining",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondsCompleted",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondsRemaining",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinutesCompleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MinutesRemaining",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecondsCompleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecondsRemaining",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TotalTime",
                table: "AspNetUsers",
                newName: "TimeSpanDuration");

            migrationBuilder.AlterColumn<double>(
                name: "HoursRemaining",
                table: "AspNetUsers",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "HoursCompleted",
                table: "AspNetUsers",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
