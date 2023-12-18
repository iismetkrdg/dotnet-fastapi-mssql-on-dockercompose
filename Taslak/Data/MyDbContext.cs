using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Taslak.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext (DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.User> User { get; set; }
        public DbSet<Models.RecommendationModel> RecommendationModel { get; set; }
        public DbSet<Models.ToSaveTracksIds> ToSaveTracksIds { get; set; }
    }
}