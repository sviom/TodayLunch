using LunchCommon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodayLunchAPI
{
    public class TodayLunchContext : DbContext
    {
        public TodayLunchContext(DbContextOptions<TodayLunchContext> options)
            : base(options)
        {

        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}
