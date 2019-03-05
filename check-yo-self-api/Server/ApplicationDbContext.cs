using System;
using check_yo_self_api.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace check_yo_self_api.Server
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    LastName = "Snodgrass",
                    FirstName = "Dick",
                    Salary = 55892.23M,
                    FirstPaycheckDate = new DateTime(2019, 1, 11)
                },
                new Employee
                {
                    EmployeeId = 2,
                    LastName = "Yubettuhdoo",
                    FirstName = "Helen",
                    Salary = 75521.55M,
                    FirstPaycheckDate = new DateTime(2018, 9, 7)
                },
                new Employee
                {
                    EmployeeId = 3,
                    LastName = "O'Lantern",
                    FirstName = "Jack",
                    Salary = 35136.28M,
                    FirstPaycheckDate = new DateTime(2018, 12, 14)
                },
                new Employee
                {
                    EmployeeId = 4,
                    LastName = "Needles",
                    FirstName = "Sharon",
                    Salary = 100001.77M,
                    FirstPaycheckDate = new DateTime(2018, 3, 9)
                },
                new Employee
                {
                    EmployeeId = 5,
                    LastName = "Dover",
                    FirstName = "Ben",
                    Salary = 82662.98M,
                    FirstPaycheckDate = new DateTime(2018, 6, 1)
                },
                new Employee
                {
                    EmployeeId = 6,
                    LastName = "Mama",
                    FirstName = "Joe",
                    Salary = 23002.44M,
                    FirstPaycheckDate = new DateTime(2018, 8, 24)
                },
                new Employee
                {
                    EmployeeId = 7,
                    LastName = "Kout",
                    FirstName = "Luke",
                    Salary = 61362.46M,
                    FirstPaycheckDate = new DateTime(2018, 1, 12)
                },
                new Employee
                {
                    EmployeeId = 8,
                    LastName = "Roni",
                    FirstName = "Pepe",
                    Salary = 103333.78M,
                    FirstPaycheckDate = new DateTime(2018, 2, 9)
                },
                new Employee
                {
                    EmployeeId = 9,
                    LastName = "Degrave",
                    FirstName = "Phil",
                    Salary = 52964.69M,
                    FirstPaycheckDate = new DateTime(2018, 4, 20)
                },
                new Employee
                {
                    EmployeeId = 10,
                    LastName = "Leakin",
                    FirstName = "Rufus",
                    Salary = 45358.72M,
                    FirstPaycheckDate = new DateTime(2018, 5, 4)
                },
                new Employee
                {
                    EmployeeId = 11,
                    LastName = "Down",
                    FirstName = "Sid",
                    Salary = 46251.25M,
                    FirstPaycheckDate = new DateTime(2018, 1, 26)
                },
                new Employee
                {
                    EmployeeId = 12,
                    LastName = "Coholic",
                    FirstName = "Al",
                    Salary = 99234.48M,
                    FirstPaycheckDate = new DateTime(2018, 6, 29)
                },
                new Employee
                {
                    EmployeeId = 13,
                    LastName = "Bath",
                    FirstName = "Anita",
                    Salary = 46223.88M,
                    FirstPaycheckDate = new DateTime(2019, 2, 22)
                },
                new Employee
                {
                    EmployeeId = 14,
                    LastName = "Butz",
                    FirstName = "Seymour",
                    Salary = 69123.25M,
                    FirstPaycheckDate = new DateTime(2018, 9, 7)
                },
                new Employee
                {
                    EmployeeId = 15,
                    LastName = "Nasium",
                    FirstName = "Jim",
                    Salary = 89221.88M,
                    FirstPaycheckDate = new DateTime(2018, 9, 21)
                },
                new Employee
                {
                    EmployeeId = 16,
                    LastName = "Kutz",
                    FirstName = "Cole",
                    Salary = 14223.84M,
                    FirstPaycheckDate = new DateTime(2018, 10, 5)
                },
                new Employee
                {
                    EmployeeId = 17,
                    LastName = "Miwords",
                    FirstName = "Mark",
                    Salary = 23445.75M,
                    FirstPaycheckDate = new DateTime(2018, 10, 19)
                },
                new Employee
                {
                    EmployeeId = 18,
                    LastName = "Lami",
                    FirstName = "Sal",
                    Salary = 150000.02M,
                    FirstPaycheckDate = new DateTime(2018, 12, 28)
                },
                new Employee
                {
                    EmployeeId = 19,
                    LastName = "Dup",
                    FirstName = "Stan",
                    Salary = 66221.85M,
                    FirstPaycheckDate = new DateTime(2018, 9, 7)
                },
                new Employee
                {
                    EmployeeId = 20,
                    LastName = "Pitt",
                    FirstName = "Stu",
                    Salary = 81268.14M,
                    FirstPaycheckDate = new DateTime(2019, 2, 22)
                }
            );
        }    
    }
}
