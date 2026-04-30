using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Smart_Parking_Garage.Persistence.migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", false, false, "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", true, false, "Member", "MEMBER" },
                    { "b1b8529f-44ad-4399-a2ee-9569551d5418", "1f40029e-885b-40e9-baf4-7a1cb4df4b23", false, false, "GarageOwner", "GARAGEOWNER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "Bookings:read", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 2, "permissions", "Bookings:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 3, "permissions", "Bookings:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 4, "permissions", "Bookings:delete", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 5, "permissions", "Chatbot:send", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 6, "permissions", "Garages:read", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 7, "permissions", "Garages:readstatus", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 8, "permissions", "Garages:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 9, "permissions", "Garages:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 10, "permissions", "Garages:delete", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 11, "permissions", "Gates:read", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 12, "permissions", "Gates:updatestatus", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 13, "permissions", "Gates:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 14, "permissions", "Gates:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 15, "permissions", "Gates:delete", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 16, "permissions", "ParkingSlots:read", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 17, "permissions", "ParkingSlots:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 18, "permissions", "ParkingSlots:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 19, "permissions", "ParkingSlots:delete", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 20, "permissions", "SensorsData:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 21, "permissions", "SensorsData:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 22, "permissions", "users:read", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 23, "permissions", "users:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 24, "permissions", "users:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 25, "permissions", "roles:read", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 26, "permissions", "roles:add", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 27, "permissions", "roles:update", "92b75286-d8f8-4061-9995-e6e23ccdee94" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1b8529f-44ad-4399-a2ee-9569551d5418");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            
        }
    }
}
