using Microsoft.EntityFrameworkCore;
using PlayGoodAssetService.Models;
using System;


namespace PlayGoodAssetService.Data
{
    internal class AssetAppDbContext : DbContext
    {
        public DbSet<AssetMetadata> AssetMetadata { get; set; }
        //public DbSet<BriefingMetadata> BriefingMetadata { get; set; }
        //public DbSet<ContentDistributionMetadata> ContentDistributionMetadata { get; set; }

        public AssetAppDbContext(DbContextOptions<AssetAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetMetadata>().HasKey(a => a.AssetId);  
                                                                          
        }
    }
}
