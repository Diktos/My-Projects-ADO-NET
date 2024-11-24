using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe.Models;
using Microsoft.EntityFrameworkCore;
namespace Cafe.Data.SQLite
{
    internal class CafeSQLiteDbContext : CafeDbContext
    {
        public CafeSQLiteDbContext(DbContextOptions<CafeSQLiteDbContext> options) : base(options)
        { 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite();
        }
    }
}
