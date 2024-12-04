using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data
{
    public abstract class CafeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ClientTable> ClientTables { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public CafeDbContext()
        {

        }

        public CafeDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(new Role[] { Role.Admin, Role.Manager, Role.Waiter });
            modelBuilder.Entity<User>().HasData(new User[] { User.Admin, User.Waiter, User.Manager, User.Barmen, User.Cook });
            modelBuilder.Entity<UserRole>().HasData(new UserRole[] { new UserRole { Id = 1, WaiterId = User.Admin.Id, RoleId = Role.Admin.Id } });
            modelBuilder.Entity<ClientTable>().HasData(ClientTable.DefaultClientTables());
            modelBuilder.Entity<Nomenclature>().HasData(Nomenclature.DefaultNomenclatures());
            modelBuilder.Entity<Order>().HasData(Order.DefaultOrders());
            modelBuilder.Entity<OrderDetail>().HasData(OrderDetail.Defaults());

        }

    }
}
