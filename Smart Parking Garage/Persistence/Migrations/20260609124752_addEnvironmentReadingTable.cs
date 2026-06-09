using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class addEnvironmentReadingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSeen",
                table: "Devices",
                newName: "TimeStamp");

            migrationBuilder.CreateTable(
                name: "EnvironmentReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Humidity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvironmentReadings_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentReadings_DeviceId",
                table: "EnvironmentReadings",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnvironmentReadings");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Devices",
                newName: "LastSeen");
        }
    }
}
