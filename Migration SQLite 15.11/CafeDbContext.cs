using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Migration_SQLite_15._11
{
    internal class CafeDbContext:Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ClientTable> ClientTables { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db");
        }
    }
}
