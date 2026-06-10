using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class AddGarageRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gates_Garages_GarageId",
                table: "Gates");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Garages_GarageId",
                table: "ParkingSlots");

            migrationBuilder.AlterColumn<int>(
                name: "GarageId",
                table: "ParkingSlots",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

           

            migrationBuilder.AlterColumn<int>(
                name: "GarageId",
                table: "Gates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gates_Garages_GarageId",
                table: "Gates",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Garages_GarageId",
                table: "ParkingSlots",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gates_Garages_GarageId",
                table: "Gates");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSlots_Garages_GarageId",
                table: "ParkingSlots");

           

            migrationBuilder.AlterColumn<int>(
                name: "GarageId",
                table: "ParkingSlots",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GarageId",
                table: "Gates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Gates_Garages_GarageId",
                table: "Gates",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSlots_Garages_GarageId",
                table: "ParkingSlots",
                column: "GarageId",
                principalTable: "Garages",
                principalColumn: "GarageId");
        }
    }
}
