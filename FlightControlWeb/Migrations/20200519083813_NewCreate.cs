using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightControlWeb.Migrations
{
    public partial class NewCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightPlans",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Passengers = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    StartLongitude = table.Column<double>(nullable: false),
                    StartLatitude = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPlans", x => x.Id);
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
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    TimeInSeconds = table.Column<int>(nullable: false),
                    FlightPlanId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_FlightPlans_FlightPlanId",
                        column: x => x.FlightPlanId,
                        principalTable: "FlightPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "FlightPlans",
                columns: new[] { "Id", "CompanyName", "Passengers", "StartDate", "StartLatitude", "StartLongitude" },
                values: new object[] { 1L, "combo", 4, new DateTime(2020, 5, 19, 11, 38, 13, 327, DateTimeKind.Local).AddTicks(8789), 32.439999999999998, 31.219999999999999 });

            migrationBuilder.InsertData(
                table: "FlightPlans",
                columns: new[] { "Id", "CompanyName", "Passengers", "StartDate", "StartLatitude", "StartLongitude" },
                values: new object[] { 2L, "mmba", 3, new DateTime(2020, 5, 19, 11, 38, 13, 331, DateTimeKind.Local).AddTicks(2111), 32.329999999999998, 31.440000000000001 });

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
                name: "Segments");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "FlightPlans");
        }
    }
}
