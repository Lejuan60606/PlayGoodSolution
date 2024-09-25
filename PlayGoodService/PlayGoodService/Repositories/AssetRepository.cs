using Microsoft.EntityFrameworkCore;
using PlayGoodService.Data;
using PlayGoodService.Models;

namespace PlayGoodService.Repositories
{
    internal class AssetRepository : IAssetRepository
    {
        private readonly AssetAppDbContext _context;
        private readonly ILogger<AssetRepository> _logger;

        public AssetRepository(AssetAppDbContext context, ILogger<AssetRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<AssetMetadata>> GetAllAssetsAsync()
        {

            try
            {
                _logger.LogInformation("Fetching all assets from the database.");
                var assetsMetadata = await _context.AssetMetadatas.ToListAsync();
                _logger.LogInformation($"Successfully retrieved {assetsMetadata.Count} assets metadata.", assetsMetadata.Count);
                return assetsMetadata;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateAssetAsync.");
                throw new Exception("Database update error.", ex);
            }
        }

        public async Task<AssetMetadata> GetAssetByIdAsync(string assetMetadataId)
        {
            try
            {
                _logger.LogInformation($"Fetching asset {assetMetadataId}.");
                var assetMetadata = await _context.AssetMetadatas.FindAsync(assetMetadataId);

                if (assetMetadata == null)
                {
                    _logger.LogWarning($"Asset metadata {assetMetadataId} not found.");
                }
                else
                {
                    _logger.LogInformation($"Successfully retrieved asset medatada {assetMetadataId}.");
                }

                return assetMetadata;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateAssetAsync.");
                throw new Exception("Database update error.", ex);
            }           
        }

        public async Task AddAssetAsync(AssetMetadata assetMetadata)
        {
            try
            {
                _logger.LogInformation($"Adding a new asset {assetMetadata.AssetId} to the database.");
                await _context.AssetMetadatas.AddAsync(assetMetadata);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully added asset metadata {assetMetadata.AssetId}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateAssetAsync.");
                throw new Exception("Database update error.", ex);
            }          
        }

        public async Task UpdateAssetAsync(AssetMetadata assetMetadata)
        {
            try
            {
                _logger.LogInformation($"Updating asset with ID {assetMetadata.AssetId}.");      
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated asset with ID {assetMetadata.AssetId}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateAssetAsync.");
                throw new Exception("Database update error.", ex);
            }         
        }

        public async Task DeleteAssetAsync(string assetId)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete asset {assetId} metadata.");
                var assetMetadata = await _context.AssetMetadatas.FindAsync(assetId);

                if (assetMetadata != null)
                {
                    _context.AssetMetadatas.Remove(assetMetadata);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Attempting to delete asset {assetId} metadata.");
                }
                else
                {
                    _logger.LogWarning($"Attempting to delete asset {assetId} metadata.");
                    throw new KeyNotFoundException("Asset not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in DeleteAssetAsync.");
                throw new Exception("Database update error.", ex);
            }          
        }
    }

}
