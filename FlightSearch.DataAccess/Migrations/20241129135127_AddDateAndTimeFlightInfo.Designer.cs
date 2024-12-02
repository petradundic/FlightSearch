﻿// <auto-generated />
using System;
using FlightSearch.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightSearch.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241129135127_AddDateAndTimeFlightInfo")]
    partial class AddDateAndTimeFlightInfo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FlightSearch.Server.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OutboundArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("OutboundDepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OutboundStops")
                        .HasColumnType("int");

                    b.Property<int>("Passengers")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ReturnDepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReturnStops")
                        .HasColumnType("int");

                    b.Property<int>("SearchParameterId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("SearchParameterId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FlightSearch.Server.Models.SearchParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Passengers")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SearchParameterHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SearchParameters");
                });

            modelBuilder.Entity("FlightSearch.Server.Models.Flight", b =>
                {
                    b.HasOne("FlightSearch.Server.Models.SearchParameter", "SearchParameter")
                        .WithMany("FlightResults")
                        .HasForeignKey("SearchParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SearchParameter");
                });

            modelBuilder.Entity("FlightSearch.Server.Models.SearchParameter", b =>
                {
                    b.Navigation("FlightResults");
                });
#pragma warning restore 612, 618
        }
    }
}
