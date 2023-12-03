using System;
using Webapi.Data.Map;
using Webapi.Model;
using Microsoft.EntityFrameworkCore;


namespace Webapi.Data
{

    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<PlayerModel> Players { get; set; }
        public DbSet<MatchModel> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchModel>().HasMany(x => x.Players).WithMany(x => x.Matches);
            modelBuilder.ApplyConfiguration(new PlayerMap());
            modelBuilder.ApplyConfiguration(new MatchMap());
            base.OnModelCreating(modelBuilder);
        }

    }
}
