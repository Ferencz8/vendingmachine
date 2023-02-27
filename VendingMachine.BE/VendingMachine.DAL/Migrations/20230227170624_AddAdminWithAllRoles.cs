using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendingMachine.DAL.Migrations
{
    public partial class AddAdminWithAllRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[,]
                {
                    { "d99cae67-145d-4ca6-b492-d9a3fbbb8921", "e10b0fb3-7044-4aa4-a156-ace92def28cd", "Seller", "SELLER" },
                    { "df164462-048c-407e-b96f-4e0e947000ee", "5fe86d98-74df-4f68-915e-39460c818c6d", "Buyer", "BUYER" },
                    { "e46fee8c-79a2-4a12-9542-695bf46629aa", "5689a319-40d7-428e-87d0-344d035d481c", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Deposit", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3440c3e5-bf42-40d4-b238-22eb3f56a84f", 0, "40cda122-0d84-4cc3-9b3b-d124d64e744f", 100, null, false, false, null, null, "FERENCZ", "AQAAAAEAACcQAAAAEEPbmDC3sJhlQHvE1z5aGAdkqr6WFr/6b+BBSQG4F8BFtBI/iiCi3kfmLWQyJD/H0Q==", null, false, "379bda3b-b9c0-47fa-81c6-62a98a13dc2e", false, "ferencz" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d99cae67-145d-4ca6-b492-d9a3fbbb8921", "3440c3e5-bf42-40d4-b238-22eb3f56a84f" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "df164462-048c-407e-b96f-4e0e947000ee", "3440c3e5-bf42-40d4-b238-22eb3f56a84f" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e46fee8c-79a2-4a12-9542-695bf46629aa", "3440c3e5-bf42-40d4-b238-22eb3f56a84f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d99cae67-145d-4ca6-b492-d9a3fbbb8921", "3440c3e5-bf42-40d4-b238-22eb3f56a84f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "df164462-048c-407e-b96f-4e0e947000ee", "3440c3e5-bf42-40d4-b238-22eb3f56a84f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e46fee8c-79a2-4a12-9542-695bf46629aa", "3440c3e5-bf42-40d4-b238-22eb3f56a84f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d99cae67-145d-4ca6-b492-d9a3fbbb8921");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df164462-048c-407e-b96f-4e0e947000ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e46fee8c-79a2-4a12-9542-695bf46629aa");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3440c3e5-bf42-40d4-b238-22eb3f56a84f");

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
    }
}
