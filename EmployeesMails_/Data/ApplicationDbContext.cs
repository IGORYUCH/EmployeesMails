using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeesMails_.Models;

namespace EmployeesMails_.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeesMails_.Models.Employee> Employee { get; set; }
        public DbSet<EmployeesMails_.Models.Mail> Mail { get; set; }


        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();

            this.Employee.Add(new Employee { Name="John", Surname="Johnson", Department="sells"});
            this.Employee.Add(new Employee { Name = "Hannah", Surname = "MacMillan", Department = "management"});
            this.Employee.Add(new Employee { Name = "Jack", Surname = "Willson", Department = "security" });
            this.Employee.Add(new Employee { Name = "Tom", Surname = "Cruise", Department = "development" });
            this.Employee.Add(new Employee { Name = "Nansy", Surname = "Willer", Department = "development" });
            this.Employee.Add(new Employee { Name = "William", Surname = "Owerbeck", Department = "security" });
            this.SaveChanges();

            this.Mail.Add(new Mail { Name = "Go home", Content = "You may go home", From_employee=this.Employee.Find(1), To_employee=this.Employee.Find(2), Date=DateTime.Now });
            this.Mail.Add(new Mail { Name = "Go home", Content = "You may go home", From_employee = this.Employee.Find(1), To_employee = this.Employee.Find(3), Date = DateTime.Now });
            this.Mail.Add(new Mail { Name = "Go home", Content = "You may go home", From_employee = this.Employee.Find(1), To_employee = this.Employee.Find(4), Date = DateTime.Now });
            this.Mail.Add(new Mail { Name = "I want to eat", Content = "Please let me go to eat. I am tired of work", From_employee = this.Employee.Find(6), To_employee = this.Employee.Find(4), Date = DateTime.Now });
            this.Mail.Add(new Mail { Name = "Is you weekend free?", Content = "So what are you doing in weekend? May be we can spend it together?:)", From_employee = this.Employee.Find(5), To_employee = this.Employee.Find(6), Date = DateTime.Now });
            this.Mail.Add(new Mail { Name = "Is you weekend free?", Content = "Sorry but i already have a boyfriend(", From_employee = this.Employee.Find(6), To_employee = this.Employee.Find(5), Date = DateTime.Now });
            this.Mail.Add(new Mail { Name = "READ URGENTLY", Content = "This is the mail without sense.", From_employee = this.Employee.Find(2), To_employee = this.Employee.Find(1), Date = DateTime.Now });
            this.SaveChanges();
        }
    }


}
