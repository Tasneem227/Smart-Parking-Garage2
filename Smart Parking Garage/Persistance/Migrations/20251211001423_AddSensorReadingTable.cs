using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.persistance.migrations
{
    /// <inheritdoc />
    public partial class AddSensorReadingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "SensorsReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    Gas = table.Column<int>(type: "int", nullable: false),
                    TotalSlots = table.Column<int>(type: "int", nullable: false),
                    OccupiedSlots = table.Column<int>(type: "int", nullable: false),
                    Slot1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3 = table.Column<bool>(type: "bit", nullable: false),
                    EntryGate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExitGate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorsReadings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "SensorsReadings");

           
        }
    }
}
