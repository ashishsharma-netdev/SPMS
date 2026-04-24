using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPMS.Migrations.InitialCreate
{
 public partial class InitialCreate : Migration
 {
 protected override void Up(MigrationBuilder migrationBuilder)
 {
 migrationBuilder.CreateTable(
 name: "Users",
 columns: table => new
 {
 Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
 Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
 PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
 Role = table.Column<int>(type: "int", nullable: false)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_Users", x => x.Id);
 });

 migrationBuilder.CreateTable(
 name: "ParkingSpaces",
 columns: table => new
 {
 ParkingSpaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
 Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
 Latitude = table.Column<double>(type: "float", nullable: false),
 Longitude = table.Column<double>(type: "float", nullable: false),
 Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
 TotalSlots = table.Column<int>(type: "int", nullable: false),
 AvailableSlots = table.Column<int>(type: "int", nullable: false),
 AreaInSqFt = table.Column<double>(type: "float", nullable: true),
 StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
 EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
 IsActive = table.Column<bool>(type: "bit", nullable: false)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_ParkingSpaces", x => x.ParkingSpaceId);
 });

 migrationBuilder.CreateTable(
 name: "ParkingSlots",
 columns: table => new
 {
 SlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 ParkingSpaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 SlotNumber = table.Column<int>(type: "int", nullable: false),
 SlotType = table.Column<int>(type: "int", nullable: false),
 IsOccupied = table.Column<bool>(type: "bit", nullable: false),
 RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_ParkingSlots", x => x.SlotId);
 });

 migrationBuilder.CreateTable(
 name: "Bookings",
 columns: table => new
 {
 BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 ParkingSpaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 SlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
 BookingType = table.Column<int>(type: "int", nullable: false),
 StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
 EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
 Status = table.Column<int>(type: "int", nullable: false),
 Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_Bookings", x => x.BookingId);
 });

 migrationBuilder.CreateTable(
 name: "Payments",
 columns: table => new
 {
 PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
 PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
 Status = table.Column<int>(type: "int", nullable: false),
 TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_Payments", x => x.PaymentId);
 });

 migrationBuilder.CreateTable(
 name: "ParkingLogs",
 columns: table => new
 {
 ParkingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
 EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
 ExitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
 CheckCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
 },
 constraints: table =>
 {
 table.PrimaryKey("PK_ParkingLogs", x => x.ParkingLogId);
 });
 }

 protected override void Down(MigrationBuilder migrationBuilder)
 {
 migrationBuilder.DropTable(name: "Users");
 migrationBuilder.DropTable(name: "ParkingSpaces");
 migrationBuilder.DropTable(name: "ParkingSlots");
 migrationBuilder.DropTable(name: "Bookings");
 migrationBuilder.DropTable(name: "Payments");
 migrationBuilder.DropTable(name: "ParkingLogs");
 }
 }
}
