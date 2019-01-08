using LunchLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchLibrary
{
    public class TodayLunchContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory) // Directory where the json files are located
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("lunchConnection"));
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}
