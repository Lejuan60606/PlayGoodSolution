
using Microsoft.EntityFrameworkCore;
using PlayGoodAssetService.Data;
using PlayGoodAssetService.Models;

namespace PlayGoodAssetService.Repositories
{
    internal class AssetRepository : IAssetRepository
    {
        private readonly AssetAppDbContext _context;

        public AssetRepository(AssetAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetMetadata>> GetAllAssetsAsync()
        {
            return await _context.AssetMetadata.ToListAsync();
        }

        public async Task<AssetMetadata> GetAssetByIdAsync(string assetId)
        {
            return await _context.AssetMetadata.FindAsync(assetId);
        }

    }
}
