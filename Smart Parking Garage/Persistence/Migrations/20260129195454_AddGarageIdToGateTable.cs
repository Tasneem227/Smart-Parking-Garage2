using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class AddGarageIdToGateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "Gates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gates_GarageId",
                table: "Gates",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gates_Garages_GarageId",
                table: "Gates",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gates_Garages_GarageId",
                table: "Gates");

            migrationBuilder.DropIndex(
                name: "IX_Gates_GarageId",
                table: "Gates");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "Gates");
        }
    }
}
