using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SPMS.Data;

namespace SPMS.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("SPMS.Models.User", b =>
            {
                b.Property<Guid>("Id").HasColumnType("uniqueidentifier");
                b.Property<string>("Email").HasColumnType("nvarchar(max)");
                b.Property<string>("Name").HasColumnType("nvarchar(max)");
                b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
                b.Property<int>("Role").HasColumnType("int");
                b.HasKey("Id");
                b.ToTable("Users");
            });

            modelBuilder.Entity("SPMS.Models.ParkingSpace", b =>
            {
                b.Property<Guid>("ParkingSpaceId").HasColumnType("uniqueidentifier");
                b.Property<Guid?>("OwnerId").HasColumnType("uniqueidentifier");
                b.Property<string>("Name").HasColumnType("nvarchar(max)");
                b.Property<double>("Latitude").HasColumnType("float");
                b.Property<double>("Longitude").HasColumnType("float");
                b.Property<string>("Address").HasColumnType("nvarchar(max)");
                b.Property<int>("TotalSlots").HasColumnType("int");
                b.Property<int>("AvailableSlots").HasColumnType("int");
                b.Property<double?>("AreaInSqFt").HasColumnType("float");
                b.Property<DateTime?>("StartDate").HasColumnType("datetime2");
                b.Property<DateTime?>("EndDate").HasColumnType("datetime2");
                b.Property<bool>("IsActive").HasColumnType("bit");
                b.HasKey("ParkingSpaceId");
                b.ToTable("ParkingSpaces");
            });

            modelBuilder.Entity("SPMS.Models.ParkingSlot", b =>
            {
                b.Property<Guid>("SlotId").HasColumnType("uniqueidentifier");
                b.Property<Guid>("ParkingSpaceId").HasColumnType("uniqueidentifier");
                b.Property<int>("SlotNumber").HasColumnType("int");
                b.Property<int>("SlotType").HasColumnType("int");
                b.Property<bool>("IsOccupied").HasColumnType("bit");
                b.Property<byte[]>("RowVersion").IsRowVersion().HasColumnType("rowversion");
                b.HasKey("SlotId");
                b.ToTable("ParkingSlots");
            });

            modelBuilder.Entity("SPMS.Models.Booking", b =>
            {
                b.Property<Guid>("BookingId").HasColumnType("uniqueidentifier");
                b.Property<Guid>("UserId").HasColumnType("uniqueidentifier");
                b.Property<Guid>("ParkingSpaceId").HasColumnType("uniqueidentifier");
                b.Property<Guid?>("SlotId").HasColumnType("uniqueidentifier");
                b.Property<int>("BookingType").HasColumnType("int");
                b.Property<DateTime>("StartTime").HasColumnType("datetime2");
                b.Property<DateTime>("EndTime").HasColumnType("datetime2");
                b.Property<int>("Status").HasColumnType("int");
                b.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                b.HasKey("BookingId");
                b.ToTable("Bookings");
            });

            modelBuilder.Entity("SPMS.Models.Payment", b =>
            {
                b.Property<Guid>("PaymentId").HasColumnType("uniqueidentifier");
                b.Property<Guid>("BookingId").HasColumnType("uniqueidentifier");
                b.Property<decimal>("Amount").HasColumnType("decimal(18,2)");
                b.Property<string>("PaymentMethod").HasColumnType("nvarchar(max)");
                b.Property<int>("Status").HasColumnType("int");
                b.Property<string>("TransactionId").HasColumnType("nvarchar(max)");
                b.HasKey("PaymentId");
                b.ToTable("Payments");
            });

            modelBuilder.Entity("SPMS.Models.ParkingLog", b =>
            {
                b.Property<Guid>("ParkingLogId").HasColumnType("uniqueidentifier");
                b.Property<Guid>("BookingId").HasColumnType("uniqueidentifier");
                b.Property<DateTime>("EntryTime").HasColumnType("datetime2");
                b.Property<DateTime?>("ExitTime").HasColumnType("datetime2");
                b.Property<string>("CheckCode").HasColumnType("nvarchar(max)");
                b.HasKey("ParkingLogId");
                b.ToTable("ParkingLogs");
            });
        }
    }
}
