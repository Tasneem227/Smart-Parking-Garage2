using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class AddGarageIdToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GarageId",
                table: "Bookings",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Garages_GarageId",
                table: "Bookings",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Garages_GarageId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GarageId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "Bookings");
        }
    }
}
