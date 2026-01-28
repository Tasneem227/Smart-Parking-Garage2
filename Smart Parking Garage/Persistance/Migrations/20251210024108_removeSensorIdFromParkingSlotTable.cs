using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.persistance.migrations
{
    /// <inheritdoc />
    public partial class removeSensorIdFromParkingSlotTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "ParkingSlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "ParkingSlots",
                type: "int",
                nullable: true);
        }
    }
}
