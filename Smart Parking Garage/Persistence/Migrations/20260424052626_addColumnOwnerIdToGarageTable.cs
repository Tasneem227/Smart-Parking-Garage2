using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class addColumnOwnerIdToGarageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Garages",
                type: "nvarchar(450)",
                nullable: true);

            
            migrationBuilder.CreateIndex(
                name: "IX_Garages_OwnerId",
                table: "Garages",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Garages_AspNetUsers_OwnerId",
                table: "Garages",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garages_AspNetUsers_OwnerId",
                table: "Garages");

            migrationBuilder.DropIndex(
                name: "IX_Garages_OwnerId",
                table: "Garages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Garages");

            
        }
    }
}
