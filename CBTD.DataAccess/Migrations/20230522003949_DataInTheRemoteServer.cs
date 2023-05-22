using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CBTDWeb.Migrations
{
    /// <inheritdoc />
    public partial class DataInTheRemoteServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2023, 5, 21, 18, 39, 49, 644, DateTimeKind.Local).AddTicks(1541));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateModified",
                value: new DateTime(2023, 5, 21, 18, 39, 49, 644, DateTimeKind.Local).AddTicks(1592));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateModified",
                value: new DateTime(2023, 5, 21, 18, 39, 49, 644, DateTimeKind.Local).AddTicks(1594));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2023, 5, 21, 16, 43, 33, 62, DateTimeKind.Local).AddTicks(8472));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateModified",
                value: new DateTime(2023, 5, 21, 16, 43, 33, 62, DateTimeKind.Local).AddTicks(8517));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateModified",
                value: new DateTime(2023, 5, 21, 16, 43, 33, 62, DateTimeKind.Local).AddTicks(8519));
        }
    }
}
