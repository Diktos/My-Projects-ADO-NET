using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Login_to_the_program__Admin__User_
{
    internal class CafeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(new Role[] {
               new Role { Id=Cafe.Models.Role.Admin.Id, Name=Cafe.Models.Role.Admin.Name },
               new Role { Id=Cafe.Models.Role.Manager.Id, Name=Cafe.Models.Role.Manager.Name },
               new Role { Id=Cafe.Models.Role.Waiter.Id, Name=Cafe.Models.Role.Waiter.Name }
            });
            modelBuilder.Entity<User>().HasData(new User[] { new User { Id=User.Admin.Id, Name=User.Admin.Name, Password=User.Admin.Password } });
            // Зв'язок користувача з роллю
            modelBuilder.Entity<UserRole>().HasData(new UserRole[] { new UserRole { Id = 1, RoleId = Cafe.Models.Role.Admin.Id, WaiterId = User.Admin.Id } }); 
        }
    }
}
