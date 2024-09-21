using Microsoft.EntityFrameworkCore;
using PlayGoodService.Data;
using PlayGoodService.Models;

namespace PlayGoodService.Repositories
{
    internal class ContentDistributionRepository : IContentDistributionRepository
    {
        private readonly AssetAppDbContext _context;
        private readonly ILogger<AssetRepository> _logger;

        public ContentDistributionRepository(AssetAppDbContext context, ILogger<AssetRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ContentDistribution>> GetAllAsync()
        {
            try
            {
                return await _context.ContentDistributionMetadatas
                .Include(cd => cd.Assets)
                .ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error.");
                throw new Exception("Database error.", ex);
            }

        }

        public async Task<ContentDistribution> GetByIdAsync(int id)
        {
            try
            {
                return await _context.ContentDistributionMetadatas
                .Include(cd => cd.Assets)
                .FirstOrDefaultAsync(cd => cd.Id == id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error.");
                throw new Exception("Database error.", ex);
            }

        }

        public async Task AddAsync(ContentDistribution contentDistribution)
        {
            try
            {
                await _context.ContentDistributionMetadatas.AddAsync(contentDistribution);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error.");
                throw new Exception("Database error.", ex);
            }

        }

        public async Task UpdateAsync(ContentDistribution contentDistribution)
        {
            var existingDistribution = await _context.ContentDistributionMetadatas.Include(cd => cd.Assets).FirstOrDefaultAsync(cd => cd.Id == contentDistribution.Id);

            if (existingDistribution == null)
                throw new KeyNotFoundException("Content Distribution not found.");

            _context.Entry(existingDistribution).CurrentValues.SetValues(contentDistribution);
            try
            {
                foreach (var existingAsset in existingDistribution.Assets.ToList())
                {
                    if (!contentDistribution.Assets.Any(a => a.AssetId == existingAsset.AssetId))
                        _context.Assets.Remove(existingAsset);
                }

                foreach (var asset in contentDistribution.Assets)
                {
                    var existingAsset = existingDistribution.Assets
                        .FirstOrDefault(a => a.AssetId == asset.AssetId);

                    if (existingAsset != null)
                    {
                        _context.Entry(existingAsset).CurrentValues.SetValues(asset);
                    }
                    else
                    {
                        existingDistribution.Assets.Add(asset);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error.");
                throw new Exception("Database error.", ex);
            }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var contentDistribution = await _context.ContentDistributionMetadatas.FindAsync(id);
                if (contentDistribution != null)
                {
                    _context.ContentDistributionMetadatas.Remove(contentDistribution);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("Content Distribution not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error.");
                throw new Exception("Database error.", ex);
            }

        }
    }
}
