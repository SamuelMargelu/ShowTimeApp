using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowTime.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingFestivalDaysDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FestivalDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FestivalDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FestivalDays_Festivals_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festivals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingFestivalDays",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    FestivalDayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFestivalDays", x => new { x.BookingId, x.FestivalDayId });
                    table.ForeignKey(
                        name: "FK_BookingFestivalDays_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingFestivalDays_FestivalDays_FestivalDayId",
                        column: x => x.FestivalDayId,
                        principalTable: "FestivalDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingFestivalDays_FestivalDayId",
                table: "BookingFestivalDays",
                column: "FestivalDayId");

            migrationBuilder.CreateIndex(
                name: "IX_FestivalDays_FestivalId",
                table: "FestivalDays",
                column: "FestivalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingFestivalDays");

            migrationBuilder.DropTable(
                name: "FestivalDays");
        }
    }
}
