using Cafe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe
{
    internal static class Config
    {
        static IConfigurationRoot? configurationRoot;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (configurationRoot == null)
                {
                    var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
                    //Determines the working environment as IHostingEnvironment is unavailable in a console app
                    var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                         devEnvironmentVariable.ToLower() == "development";
                    var builder = new ConfigurationBuilder();
                    // tell the builder to look for the appsettings.json file
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    //only add secrets in development
                    if (isDevelopment)
                    {
                        builder.AddUserSecrets<App>();
                    }
                    configurationRoot = builder.Build();
                    //dbProvider = configurationRoot["DbProvider"];

                }
                return configurationRoot;
            }
        }

        private static string? dbProvider;

        static Config()
        {
            using (var context = GetDbContext())
            {
                context.Database.Migrate();
            }
        }

        public static CafeDbContext GetDbContext()
        {
            return new Cafe.Data.CafeSQLiteDbContext();
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //dbProvider = Configuration["DbProvider"];
            //if (dbProvider != null)
            //{
            //    //switch (dbProvider)
            //    //{
            //    //    //case "SQLite": return new Cafe.Data.CafeSQLiteDbContext(connectionString);
            //    //    //case "MSSQL": return new Cafe.Data.CafeMSSQLDbContext();
            //    //    default: throw new Exception($"The {dbProvider} is not support!");


            //    //}
            //}
            //throw new Exception($"Is not defined parametr DbProvider in appsettings.json!");
        }


        public static int UserId { get; set; }
        public static string UserName { get; set; }


    }
}
