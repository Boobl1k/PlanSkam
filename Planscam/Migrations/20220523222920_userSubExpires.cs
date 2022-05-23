using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planscam.Migrations
{
    public partial class userSubExpires : Migration
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
                name: "SubExpires",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1aa278af-7fd5-4d1f-9f15-51d4983eefb1", "ca247a30-f436-4566-822a-835720a7678f", "Sub", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2d912b43-84c4-460e-bbfd-9bb16d1bb4e3", "4401abca-6ed9-4fbd-a073-b1ea46c29562", "Author", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1aa278af-7fd5-4d1f-9f15-51d4983eefb1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d912b43-84c4-460e-bbfd-9bb16d1bb4e3");

            migrationBuilder.DropColumn(
                name: "SubExpires",
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
