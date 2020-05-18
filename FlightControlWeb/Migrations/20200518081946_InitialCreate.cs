using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightControlWeb.Migrations
{
    public partial class InitialCreate : Migration
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
                name: "Segment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    TimeInSeconds = table.Column<int>(nullable: false),
                    FlightPlanId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segment_FlightPlans_FlightPlanId",
                        column: x => x.FlightPlanId,
                        principalTable: "FlightPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "FlightPlans",
                columns: new[] { "Id", "CompanyName", "Passengers", "StartDate", "StartLatitude", "StartLongitude" },
                values: new object[] { 1L, "combo", 4, new DateTime(2020, 5, 18, 11, 19, 46, 314, DateTimeKind.Local).AddTicks(9957), 32.439999999999998, 31.219999999999999 });

            migrationBuilder.InsertData(
                table: "FlightPlans",
                columns: new[] { "Id", "CompanyName", "Passengers", "StartDate", "StartLatitude", "StartLongitude" },
                values: new object[] { 2L, "mmba", 3, new DateTime(2020, 5, 18, 11, 19, 46, 323, DateTimeKind.Local).AddTicks(9025), 32.329999999999998, 31.440000000000001 });

            migrationBuilder.CreateIndex(
                name: "IX_Segment_FlightPlanId",
                table: "Segment",
                column: "FlightPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Segment");

            migrationBuilder.DropTable(
                name: "FlightPlans");
        }
    }
}
