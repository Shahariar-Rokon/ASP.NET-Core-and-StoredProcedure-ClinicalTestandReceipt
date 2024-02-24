using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooAppUsingSp.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    TotalBill = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TestUnitId = table.Column<int>(type: "int", nullable: false),
                    TestUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TestTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestDetails_ClientHeaders_ClientHeaderId",
                        column: x => x.ClientHeaderId,
                        principalTable: "ClientHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestDetails_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestDetails_TestUnits_TestUnitId",
                        column: x => x.TestUnitId,
                        principalTable: "TestUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_ClientHeaderId",
                table: "TestDetails",
                column: "ClientHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_TestId",
                table: "TestDetails",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_TestUnitId",
                table: "TestDetails",
                column: "TestUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestDetails");

            migrationBuilder.DropTable(
                name: "ClientHeaders");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "TestUnits");
        }
    }
}
