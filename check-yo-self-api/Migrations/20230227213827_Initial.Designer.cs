﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using check_yo_self_api.Server;

#nullable disable

namespace checkyoselfapi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230227213827_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("check_yo_self_api.Server.Entities.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<DateTime>("FirstPaycheckDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            FirstName = "Dick",
                            FirstPaycheckDate = new DateTime(2019, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Snodgrass",
                            Salary = 55892.23m
                        },
                        new
                        {
                            EmployeeId = 2,
                            FirstName = "Helen",
                            FirstPaycheckDate = new DateTime(2018, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Yubettuhdoo",
                            Salary = 75521.55m
                        },
                        new
                        {
                            EmployeeId = 3,
                            FirstName = "Jack",
                            FirstPaycheckDate = new DateTime(2018, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "O'Lantern",
                            Salary = 35136.28m
                        },
                        new
                        {
                            EmployeeId = 4,
                            FirstName = "Sharon",
                            FirstPaycheckDate = new DateTime(2018, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Needles",
                            Salary = 100001.77m
                        },
                        new
                        {
                            EmployeeId = 5,
                            FirstName = "Ben",
                            FirstPaycheckDate = new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Dover",
                            Salary = 82662.98m
                        },
                        new
                        {
                            EmployeeId = 6,
                            FirstName = "Joe",
                            FirstPaycheckDate = new DateTime(2018, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Mama",
                            Salary = 23002.44m
                        },
                        new
                        {
                            EmployeeId = 7,
                            FirstName = "Luke",
                            FirstPaycheckDate = new DateTime(2018, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Kout",
                            Salary = 61362.46m
                        },
                        new
                        {
                            EmployeeId = 8,
                            FirstName = "Pepe",
                            FirstPaycheckDate = new DateTime(2018, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Roni",
                            Salary = 103333.78m
                        },
                        new
                        {
                            EmployeeId = 9,
                            FirstName = "Phil",
                            FirstPaycheckDate = new DateTime(2018, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Degrave",
                            Salary = 52964.69m
                        },
                        new
                        {
                            EmployeeId = 10,
                            FirstName = "Rufus",
                            FirstPaycheckDate = new DateTime(2018, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Leakin",
                            Salary = 45358.72m
                        },
                        new
                        {
                            EmployeeId = 11,
                            FirstName = "Sid",
                            FirstPaycheckDate = new DateTime(2018, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Down",
                            Salary = 46251.25m
                        },
                        new
                        {
                            EmployeeId = 12,
                            FirstName = "Al",
                            FirstPaycheckDate = new DateTime(2018, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Coholic",
                            Salary = 99234.48m
                        },
                        new
                        {
                            EmployeeId = 13,
                            FirstName = "Anita",
                            FirstPaycheckDate = new DateTime(2019, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Bath",
                            Salary = 46223.88m
                        },
                        new
                        {
                            EmployeeId = 14,
                            FirstName = "Seymour",
                            FirstPaycheckDate = new DateTime(2018, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Butz",
                            Salary = 69123.25m
                        },
                        new
                        {
                            EmployeeId = 15,
                            FirstName = "Jim",
                            FirstPaycheckDate = new DateTime(2018, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nasium",
                            Salary = 89221.88m
                        },
                        new
                        {
                            EmployeeId = 16,
                            FirstName = "Cole",
                            FirstPaycheckDate = new DateTime(2018, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Kutz",
                            Salary = 14223.84m
                        },
                        new
                        {
                            EmployeeId = 17,
                            FirstName = "Mark",
                            FirstPaycheckDate = new DateTime(2018, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Miwords",
                            Salary = 23445.75m
                        },
                        new
                        {
                            EmployeeId = 18,
                            FirstName = "Sal",
                            FirstPaycheckDate = new DateTime(2018, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Lami",
                            Salary = 150000.02m
                        },
                        new
                        {
                            EmployeeId = 19,
                            FirstName = "Stan",
                            FirstPaycheckDate = new DateTime(2018, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Dup",
                            Salary = 66221.85m
                        },
                        new
                        {
                            EmployeeId = 20,
                            FirstName = "Stu",
                            FirstPaycheckDate = new DateTime(2019, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Pitt",
                            Salary = 81268.14m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
