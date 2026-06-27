using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class CaptureImageTableimageurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "CapturedImages");

            migrationBuilder.DropColumn(
                name: "ImageExtension",
                table: "CapturedImages");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "CapturedImages");

            migrationBuilder.RenameColumn(
                name: "StoredImageName",
                table: "CapturedImages",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "CapturedImages",
                newName: "StoredImageName");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "CapturedImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageExtension",
                table: "CapturedImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "CapturedImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
