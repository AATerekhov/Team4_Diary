﻿// <auto-generated />
using System;
using Diary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Diary.DataAccess.Migrations
{
    [DbContext(typeof(EfDbContext))]
    [Migration("20250112110411_addedHabitAndHabitStateEntity")]
    partial class addedHabitAndHabitStateEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Diary.Core.Domain.Administration.HabitDiaryOwner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("DiaryOwnerId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("HabitDiaryOwners");
                });

            modelBuilder.Entity("Diary.Core.Domain.Diary.HabitDiary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("DiaryId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("DiaryOwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("DiaryOwnerId");

                    b.ToTable("HabitDiaries");
                });

            modelBuilder.Entity("Diary.Core.Domain.Diary.HabitDiaryLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("DiaryLineId");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DiaryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("entityType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DiaryId");

                    b.ToTable("HabitDiaryLines");
                });

            modelBuilder.Entity("Diary.Core.Domain.Habits.Habit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("HabitId");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("DiaryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DiaryId");

                    b.ToTable("Habits");
                });

            modelBuilder.Entity("Diary.Core.Domain.Habits.HabitState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("HabitStateId");

                    b.Property<Guid>("HabitId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsNotified")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Tag")
                        .HasColumnType("integer");

                    b.Property<int>("TextNegative")
                        .HasColumnType("integer");

                    b.Property<int>("TextPositive")
                        .HasColumnType("integer");

                    b.Property<bool>("TitleCheck")
                        .HasColumnType("boolean");

                    b.Property<string>("TitleReportField")
                        .HasColumnType("text");

                    b.Property<int>("TitleValue")
                        .HasColumnType("integer");

                    b.Property<bool>("isRated")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("HabitId");

                    b.ToTable("HabitStates");
                });

            modelBuilder.Entity("Diary.Core.Domain.Diary.HabitDiary", b =>
                {
                    b.HasOne("Diary.Core.Domain.Administration.HabitDiaryOwner", "DiaryOwner")
                        .WithMany("Diaries")
                        .HasForeignKey("DiaryOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaryOwner");
                });

            modelBuilder.Entity("Diary.Core.Domain.Diary.HabitDiaryLine", b =>
                {
                    b.HasOne("Diary.Core.Domain.Diary.HabitDiary", "Diary")
                        .WithMany("Lines")
                        .HasForeignKey("DiaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diary");
                });

            modelBuilder.Entity("Diary.Core.Domain.Habits.Habit", b =>
                {
                    b.HasOne("Diary.Core.Domain.Diary.HabitDiary", "Diary")
                        .WithMany("Habits")
                        .HasForeignKey("DiaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diary");
                });

            modelBuilder.Entity("Diary.Core.Domain.Habits.HabitState", b =>
                {
                    b.HasOne("Diary.Core.Domain.Habits.Habit", "Habit")
                        .WithMany("HabitStates")
                        .HasForeignKey("HabitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Habit");
                });

            modelBuilder.Entity("Diary.Core.Domain.Administration.HabitDiaryOwner", b =>
                {
                    b.Navigation("Diaries");
                });

            modelBuilder.Entity("Diary.Core.Domain.Diary.HabitDiary", b =>
                {
                    b.Navigation("Habits");

                    b.Navigation("Lines");
                });

            modelBuilder.Entity("Diary.Core.Domain.Habits.Habit", b =>
                {
                    b.Navigation("HabitStates");
                });
#pragma warning restore 612, 618
        }
    }
}
