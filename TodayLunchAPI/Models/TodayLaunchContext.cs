using LunchCommon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodayLunchAPI.Models
{
    public class TodayLunchContext : DbContext
    {
        public TodayLunchContext(DbContextOptions<TodayLunchContext> options)
            : base(options)
        {
            // TODO: #639
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Place> Places { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}
