using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class AddDeviceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GarageId = table.Column<int>(type: "int", nullable: false),
                    SlotsCount = table.Column<int>(type: "int", nullable: false),
                    HasCamera = table.Column<bool>(type: "bit", nullable: false),
                    HasEnvSensors = table.Column<bool>(type: "bit", nullable: false),
                    HasEntryGate = table.Column<bool>(type: "bit", nullable: false),
                    HasExitGate = table.Column<bool>(type: "bit", nullable: false),
                    LastSeen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_Garages_GarageId",
                        column: x => x.GarageId,
                        principalTable: "Garages",
                        principalColumn: "GarageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_GarageId",
                table: "Devices",
                column: "GarageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
