using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EFCore_Demo
{
    internal class CafeDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(new Role[] { Role.Admin, Role.Manager, Role.Waiter });
            modelBuilder.Entity<User>().HasData(new User[] { User.Admin });
            modelBuilder.Entity<UserRole>().HasData(new UserRole[] { new UserRole { Id = 1, UserId = User.Admin.Id, RoleId = Role.Admin.Id } });
        }
    }
}
