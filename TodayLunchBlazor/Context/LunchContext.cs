using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LunchLibrary.Models;
using LunchLibrary;

namespace TodayLunchBlazor.Context
{
    public class LunchContext : DbContext
    {
        public DbSet<Place> Places { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            var endpoint = UtilityLauncher.GetKeyVaultSecretAsync("LunchCosmosEndpoint").Result;
            var accountKey = UtilityLauncher.GetKeyVaultSecretAsync("LunchCosmosPKey").Result;

            optionsBuilder.UseCosmos(endpoint, accountKey, "LunchItems");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultContainer("LunchItems");
        }
    }
}
