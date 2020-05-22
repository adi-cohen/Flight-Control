using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightControlWeb.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Passengers = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IsExternal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                });

            migrationBuilder.CreateTable(
                name: "FlightPlan",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Passengers = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InitialLocation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<long>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<long>(nullable: false),
                    SegmentNumber = table.Column<long>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    TimeInSeconds = table.Column<int>(nullable: false),
                    FlightPlanId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_FlightPlan_FlightPlanId",
                        column: x => x.FlightPlanId,
                        principalTable: "FlightPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Servers",
                columns: new[] { "Id", "Url" },
                values: new object[] { 1L, "testURL.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_FlightPlanId",
                table: "Segments",
                column: "FlightPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "InitialLocation");

            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "FlightPlan");
        }
    }
}
