using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendingMachine.DAL.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85a7befd-71e2-4c4d-980e-9e26db2b1ce6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a174dccb-c758-4cbf-812e-548803c58873");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9046db65-a55f-4f3c-bf85-00bffcbd2d7c", "e55e27eb-b703-40bb-a47c-0e1b58b62337", "Admin", "ADMIN" },
                    { "93512b7a-1f8a-4529-9b4d-65405bfee0f6", "3bb7ec9b-a185-4d4c-9dee-331d0a4946e1", "Seller", "SELLER" },
                    { "a10c438b-42f8-4605-9963-5c23f7ac8bba", "2bc13de7-ac3c-498a-9636-e2687bb88ea0", "Buyer", "BUYER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Deposit", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7aee0ae3-4c1d-4042-8aa4-d8706055feae", 0, "098ba983-0be6-483c-a455-fdf7fc256c90", 100, null, false, false, null, null, "FERENCZ", "AQAAAAEAACcQAAAAEAodwl571t4ED20vBcWzNgPRZZ083hYLCuxrNyay002xc4GVv2yiQUdofQknObMD/g==", null, false, "ffeba8f2-2527-4e00-a09b-0a394a2a73a9", false, "ferencz" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9046db65-a55f-4f3c-bf85-00bffcbd2d7c", "7aee0ae3-4c1d-4042-8aa4-d8706055feae" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93512b7a-1f8a-4529-9b4d-65405bfee0f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a10c438b-42f8-4605-9963-5c23f7ac8bba");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9046db65-a55f-4f3c-bf85-00bffcbd2d7c", "7aee0ae3-4c1d-4042-8aa4-d8706055feae" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9046db65-a55f-4f3c-bf85-00bffcbd2d7c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7aee0ae3-4c1d-4042-8aa4-d8706055feae");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85a7befd-71e2-4c4d-980e-9e26db2b1ce6", "b705dab2-5da8-4bed-8f1a-f33efcd37f7c", "Seller", "SELLER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a174dccb-c758-4cbf-812e-548803c58873", "dc365188-82b3-4fe9-ac9a-4b70bc09926b", "Buyer", "BUYER" });
        }
    }
}
