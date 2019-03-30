﻿using LunchLibrary.Models;
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
            // appsettings.json 파일 사용
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("lunchConnection"));

            if (string.IsNullOrEmpty(GooglePlatform.PlaceAPI.API_KEY))
            {
                var apiKey = configuration.GetSection("Google")["PlaceApiKey"];
                GooglePlatform.PlaceAPI.API_KEY = apiKey;
            }
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
