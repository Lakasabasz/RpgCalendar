﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RpgCalendar.Database;

#nullable disable

namespace RpgCalendar.Database.Migrations
{
    [DbContext(typeof(RelationalDb))]
    [Migration("20240819103021_PrivateEvents")]
    partial class PrivateEvents
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("RpgCalendar.Database.Models.PrivateEvent", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("SimpleAbsence")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("EventId");

                    b.HasIndex("OwnerId");

                    b.ToTable("PrivateEvents");
                });

            modelBuilder.Entity("RpgCalendar.Database.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Nick")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("PrivateCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.HasKey("Id");

                    b.HasIndex("PrivateCode")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RpgCalendar.Database.Models.PrivateEvent", b =>
                {
                    b.HasOne("RpgCalendar.Database.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });
#pragma warning restore 612, 618
        }
    }
}
