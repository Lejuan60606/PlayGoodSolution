using Microsoft.EntityFrameworkCore;
using PlayGoodService.Models;


namespace PlayGoodService.Data
{
    internal class AssetAppDbContext : DbContext
    {
        public DbSet<AssetMetadata> AssetMetadatas { get; set; }
        public DbSet<BriefingMetadata> BriefingMetadatas { get; set; }
        public DbSet<ContentDistribution> ContentDistributionMetadatas { get; set; }
        public DbSet<Asset> Assets { get; set; }


        public AssetAppDbContext(DbContextOptions<AssetAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetMetadata>().HasKey(a => a.AssetId);
            modelBuilder.Entity<BriefingMetadata>().HasKey(a => a.AssetId);
            modelBuilder.Entity<Asset>().HasKey(a => a.AssetId);
            modelBuilder.Entity<ContentDistribution>()
                .HasMany(cd => cd.Assets)
                .WithOne(a => a.ContentDistribution)
                .HasForeignKey(a => a.ContentDistributionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
