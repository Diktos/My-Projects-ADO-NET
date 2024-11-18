using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreMigrationDemo
{
    partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(name: "Waiters", columns: table => new
            {
                Id = table.Column<int>(nullable: false),
                Name = table.Column<string>(type:"TEXT", nullable: false)
            }
            );
        }
    }
}
