using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace Cafe.Data.SQLite
{
    public class CafeSQLiteDbContext : CafeDbContext
    {
        string _connectionString;

        //public CafeSQLiteDbContext(DbContextOptions options) : base(options)
        //{
        //}

        //public CafeSQLiteDbContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
