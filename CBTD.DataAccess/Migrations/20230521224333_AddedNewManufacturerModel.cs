using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CBTDWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewManufacturerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Coca-Cola" },
                    { 2, "Vineyard" },
                    { 3, "oreilly" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateModified",
                value: new DateTime(2023, 5, 12, 14, 2, 27, 624, DateTimeKind.Local).AddTicks(4914));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateModified",
                value: new DateTime(2023, 5, 12, 14, 2, 27, 624, DateTimeKind.Local).AddTicks(4965));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateModified",
                value: new DateTime(2023, 5, 12, 14, 2, 27, 624, DateTimeKind.Local).AddTicks(4967));
        }
    }
}
