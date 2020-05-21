﻿// <auto-generated />
using System;
using FlightControlWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlightControlWeb.Migrations
{
    [DbContext(typeof(DBInteractor))]
    partial class DBInteractorModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("FlightControlWeb.Models.ExternalFlight", b =>
                {
                    b.Property<long>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalServerUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("FlightId");

                    b.ToTable("ExternalFlights");
                });

            modelBuilder.Entity("FlightControlWeb.Models.FlightPlan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Passengers")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("FlightPlan");
                });

            modelBuilder.Entity("FlightControlWeb.Models.InitialLocation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("InitialLocation");
                });

            modelBuilder.Entity("FlightControlWeb.Models.Segment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("FlightPlanId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<long>("SegmentNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeInSeconds")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FlightPlanId");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("FlightControlWeb.Models.Server", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("FlightControlWeb.Models.Segment", b =>
                {
                    b.HasOne("FlightControlWeb.Models.FlightPlan", null)
                        .WithMany("Segments")
                        .HasForeignKey("FlightPlanId");
                });
#pragma warning restore 612, 618
        }
    }
}
