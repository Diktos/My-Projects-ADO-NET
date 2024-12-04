using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public DbSet<ClientTable> ClientTables { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public static IConfiguration Configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .AddUserSecrets<Program>().Build();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
            //Action<String> action;
            //Func<string> func;
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("SQLIte"));
            optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging(true);

        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Trace);
        });
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(new Role[] { Role.Admin, Role.Manager, Role.Waiter });
            modelBuilder.Entity<User>().HasData(new User[] { User.Admin });
            modelBuilder.Entity<UserRole>().HasData(new UserRole[] { new UserRole { Id = 1, WaiterId = User.Admin.Id, RoleId = Role.Admin.Id } });
        }
    }
}
