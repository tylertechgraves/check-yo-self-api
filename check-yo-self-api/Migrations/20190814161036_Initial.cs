using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace checkyoselfapi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    LastName = table.Column<string>(maxLength: 1024, nullable: false),
                    FirstName = table.Column<string>(maxLength: 1024, nullable: false),
                    Salary = table.Column<decimal>(nullable: false),
                    FirstPaycheckDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "FirstName", "FirstPaycheckDate", "LastName", "Salary" },
                values: new object[,]
                {
                    { 1, "Dick", new DateTime(2019, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Snodgrass", 55892.23m },
                    { 18, "Sal", new DateTime(2018, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lami", 150000.02m },
                    { 17, "Mark", new DateTime(2018, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Miwords", 23445.75m },
                    { 16, "Cole", new DateTime(2018, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kutz", 14223.84m },
                    { 15, "Jim", new DateTime(2018, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nasium", 89221.88m },
                    { 14, "Seymour", new DateTime(2018, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Butz", 69123.25m },
                    { 13, "Anita", new DateTime(2019, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bath", 46223.88m },
                    { 12, "Al", new DateTime(2018, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coholic", 99234.48m },
                    { 11, "Sid", new DateTime(2018, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Down", 46251.25m },
                    { 10, "Rufus", new DateTime(2018, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leakin", 45358.72m },
                    { 9, "Phil", new DateTime(2018, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Degrave", 52964.69m },
                    { 8, "Pepe", new DateTime(2018, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Roni", 103333.78m },
                    { 7, "Luke", new DateTime(2018, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kout", 61362.46m },
                    { 6, "Joe", new DateTime(2018, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mama", 23002.44m },
                    { 5, "Ben", new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dover", 82662.98m },
                    { 4, "Sharon", new DateTime(2018, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Needles", 100001.77m },
                    { 3, "Jack", new DateTime(2018, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "O'Lantern", 35136.28m },
                    { 2, "Helen", new DateTime(2018, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yubettuhdoo", 75521.55m },
                    { 19, "Stan", new DateTime(2018, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dup", 66221.85m },
                    { 20, "Stu", new DateTime(2019, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pitt", 81268.14m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
