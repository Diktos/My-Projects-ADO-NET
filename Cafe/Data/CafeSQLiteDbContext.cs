
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cafe.Data
{
    public class CafeSQLiteDbContext : CafeDbContext
    {
        string _connectionString;
        //public CafeSQLiteDbContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}
       // public CafeSQLiteDbContext(DbContextOptions<CafeSQLiteDbContext> options)
       //: base(options)
       // {

       // }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            // Якщо рядок підключення не передано 
            //optionsBuilder.UseSqlite("DefaultConnection");
            optionsBuilder.UseSqlite(Config.Configuration.GetConnectionString("DefaultConnection"));
            }
             base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}