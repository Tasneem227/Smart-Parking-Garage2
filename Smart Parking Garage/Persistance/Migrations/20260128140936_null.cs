using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.persistance.migrations
{
    /// <inheritdoc />
    public partial class @null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Garage_GarageId",
                table: "ParkingSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Garage",
                table: "Garage");

            migrationBuilder.RenameTable(
                name: "Garage",
                newName: "Garages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Garages",
                table: "Garages",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Garages_GarageId",
                table: "ParkingSlots",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Garages_GarageId",
                table: "ParkingSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Garages",
                table: "Garages");

            migrationBuilder.RenameTable(
                name: "Garages",
                newName: "Garage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Garage",
                table: "Garage",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Garage_GarageId",
                table: "ParkingSlots",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "GarageId");
        }
    }
}
