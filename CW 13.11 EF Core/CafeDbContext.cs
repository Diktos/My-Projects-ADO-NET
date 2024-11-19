using Cafe.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_13._11_EF_Core
{
    public class CafeDbContext:DbContext
    {
        public DbSet<User> Waiters { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("DataSource=C:/Users/Danylo/Desktop/My Project ADO NET/cafe_12.db");
        }
    }
}
