using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightSearch.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDateAndTimeFlightInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureDate",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "Flights",
                newName: "ReturnDepartureTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "OutboundArrivalTime",
                table: "Flights",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OutboundDepartureTime",
                table: "Flights",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnArrivalTime",
                table: "Flights",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutboundArrivalTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "OutboundDepartureTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ReturnArrivalTime",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "ReturnDepartureTime",
                table: "Flights",
                newName: "ReturnDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDate",
                table: "Flights",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
