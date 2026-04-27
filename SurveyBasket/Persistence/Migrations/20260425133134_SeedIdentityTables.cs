using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019dc466-25e1-72e3-8121-9500e0275083", "019dc466-25e1-72e3-8121-9501313f1be4", false, false, "Admin", "ADMIN" },
                    { "019dc466-25e1-72e3-8121-950206cc2045", "019dc466-25e1-72e3-8121-9503ddd31e68", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019db54e-da0e-7efa-bfec-f6a326dd7711", 0, "019db54e-da0e-7efa-bfec-f6a447c0ef94", "admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEDy51N2WzJaEB5ah+notG/iayHmMwpbCETYKoKB6aGVmIqePYhFoJ63zbA1hyaR5rA==", null, false, "41D236D0CC0A4C518263E4B3E9E86DE9", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permission", "polls:read", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 2, "permission", "polls:add", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 3, "permission", "polls:update", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 4, "permission", "polls:delete", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 5, "permission", "questions:read", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 6, "permission", "questions:add", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 7, "permission", "questions:update", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 8, "permission", "users:read", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 9, "permission", "users:add", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 10, "permission", "users:update", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 11, "permission", "roles:read", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 12, "permission", "roles:add", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 13, "permission", "roles:update", "019dc466-25e1-72e3-8121-9500e0275083" },
                    { 14, "permission", "results:read", "019dc466-25e1-72e3-8121-9500e0275083" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019dc466-25e1-72e3-8121-9500e0275083", "019db54e-da0e-7efa-bfec-f6a326dd7711" });
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
                keyValue: "019dc466-25e1-72e3-8121-950206cc2045");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019dc466-25e1-72e3-8121-9500e0275083", "019db54e-da0e-7efa-bfec-f6a326dd7711" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019dc466-25e1-72e3-8121-9500e0275083");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019db54e-da0e-7efa-bfec-f6a326dd7711");
        }
    }
}
