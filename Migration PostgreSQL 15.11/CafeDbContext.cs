using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Migration_SQLite_15._11
{
    internal class CafeDbContext : DbContext
    {
        public static IConfiguration Configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .AddUserSecrets<Program>().Build();

        public DbSet<ClientTable> ClientTables { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostgreSql"));
        }
    }
}
