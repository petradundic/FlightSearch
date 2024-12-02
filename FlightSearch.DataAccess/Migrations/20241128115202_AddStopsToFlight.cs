using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightSearch.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStopsToFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stops",
                table: "Flights",
                newName: "ReturnStops");

            migrationBuilder.AlterColumn<string>(
                name: "SearchParameterHash",
                table: "SearchParameters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OutboundStops",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutboundStops",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "ReturnStops",
                table: "Flights",
                newName: "Stops");

            migrationBuilder.AlterColumn<string>(
                name: "SearchParameterHash",
                table: "SearchParameters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
