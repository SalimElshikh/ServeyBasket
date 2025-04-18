using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e974a5c-7f68-418e-8c69-2d04acda02d2", "fb02c718-652b-4872-b0eb-c405a27f8d0b", true, false, "Member", "MEMBER" },
                    { "e09ce402-2bd5-4cb9-a70c-a779f486051e", "9d6c20b8-0e1c-4ae1-9d30-801b112943a2", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6ca53e83-c8d3-4be3-96c5-f37262b61d64", 0, "2bc3679c-1d6b-47c9-a49c-5c6fae406e36", "admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "SALIMELSHIKH", "AQAAAAIAAYagAAAAEFmUmJ5Vw84/KZ6/IViUcFfuQm3Q4sUAXcjBy7pTWUJHnHzEoKD8wY+MlbEfUHpr+w==", null, false, "53ECABCEAAF84247B88CE527D5135B97", false, "SalimElshikh" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permission", "polls:read", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 2, "permission", "polls:add", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 3, "permission", "polls:update", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 4, "permission", "polls:delete", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 5, "permission", "questions:read", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 6, "permission", "questions:add", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 7, "permission", "questions:update", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 8, "permission", "user:read", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 9, "permission", "user:add", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 10, "permission", "user:update", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 11, "permission", "role:read", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 12, "permission", "role:add", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 13, "permission", "role:update", "e09ce402-2bd5-4cb9-a70c-a779f486051e" },
                    { 14, "permission", "result:read", "e09ce402-2bd5-4cb9-a70c-a779f486051e" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e09ce402-2bd5-4cb9-a70c-a779f486051e", "6ca53e83-c8d3-4be3-96c5-f37262b61d64" });
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
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e974a5c-7f68-418e-8c69-2d04acda02d2");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e09ce402-2bd5-4cb9-a70c-a779f486051e", "6ca53e83-c8d3-4be3-96c5-f37262b61d64" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e09ce402-2bd5-4cb9-a70c-a779f486051e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6ca53e83-c8d3-4be3-96c5-f37262b61d64");
        }
    }
}
