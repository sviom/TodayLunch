using LunchLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LunchLibrary
{
    public class TodayLunchContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var secretConnection = UtilityLauncher.GetKeyVaultSecretAsync("LunchConnectionString").Result;
                var secretGooleApiKey = UtilityLauncher.GetKeyVaultSecretAsync("GooglePlaceApiKey").Result;

                // appsettings.json 파일 사용
                //IConfiguration configuration = new ConfigurationBuilder()
                //    .SetBasePath(Environment.CurrentDirectory)
                //    .AddJsonFile("appsettings_core.json", optional: false, reloadOnChange: true)
                //    .Build();

                //optionsBuilder.UseSqlServer(configuration.GetConnectionString("lunchConnection"));
                optionsBuilder.UseSqlServer(secretConnection);

                if (string.IsNullOrEmpty(GooglePlatform.PlaceAPI.API_KEY))
                {
                    //var apiKey = configuration.GetSection("Google")["PlaceApiKey"];
                    GooglePlatform.PlaceAPI.API_KEY = secretGooleApiKey;
                }
            }
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
