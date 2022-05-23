using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planscam.Migrations
{
    public partial class userSubInspires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b2ae3b3-fb58-4a25-bf4f-a59e4da261da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b952171-1ae0-4349-a67c-ec1109b7f525");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubInspires",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "32c565ce-2fb6-4cd5-b132-09b0b8b7833f", "1bd0b8fe-de34-48a4-bcd6-dfb20152c304", "Author", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4fae79da-aa34-4917-ad20-a9db5034f5f2", "2b0e519d-1343-41bf-b607-72e5a5d03807", "Sub", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32c565ce-2fb6-4cd5-b132-09b0b8b7833f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4fae79da-aa34-4917-ad20-a9db5034f5f2");

            migrationBuilder.DropColumn(
                name: "SubInspires",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2b2ae3b3-fb58-4a25-bf4f-a59e4da261da", "8bf83fbe-3c20-4495-9c65-f7bf55e98c0b", "Sub", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5b952171-1ae0-4349-a67c-ec1109b7f525", "95173b8f-4abf-4f1b-9179-24faaae76160", "Author", null });
        }
    }
}
