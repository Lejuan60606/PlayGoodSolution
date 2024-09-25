
using Microsoft.EntityFrameworkCore;
using PlayGoodService.Models;
using PlayGoodService.Repositories;

namespace PlayGoodService.Services
{
    internal class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;
        private readonly ILogger<AssetService> _logger;

        public AssetService(IAssetRepository repository, ILogger<AssetService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<AssetMetadata>> GetAllAssetsAsync()
        {
            _logger.LogDebug("Getting all assets from repository.");
            return await _repository.GetAllAssetsAsync();
        }

        public async Task<AssetMetadata> GetAssetByIdAsync(string assetId)
        {
            _logger.LogDebug($"Getting asset {assetId} from repository.");
            return await _repository.GetAssetByIdAsync(assetId);
        }

        public async Task AddAssetAsync(AssetMetadata asset)
        {
            _logger.LogInformation($"Adding new asset {asset.AssetId}");
            await _repository.AddAssetAsync(asset);
        }

        public async Task UpdateAssetAsync(string assetId, AssetMetadata asset)
        {
            if (assetId != asset.AssetId)
            {
                _logger.LogWarning("Asset ID mismatch in update operation.");
                throw new ArgumentException("Asset ID mismatch.");
            }

            var existingAsset = await _repository.GetAssetByIdAsync(assetId);
            if (existingAsset == null)
            {
                _logger.LogWarning($"Asset {assetId} not found for update.");
                throw new KeyNotFoundException("Asset not found.");
            }

            UpdateAssetFields(existingAsset, asset);

            _logger.LogInformation($"Updating asset {assetId}");
            await _repository.UpdateAssetAsync(existingAsset);
        }

        public async Task DeleteAssetAsync(string assetId)
        {
            _logger.LogInformation($"Deleting asset {assetId}");

            var existingAsset =  await _repository.GetAssetByIdAsync(assetId);

            if (existingAsset == null)
            {
                _logger.LogWarning($"Asset {assetId} not found for update.");
                throw new KeyNotFoundException("Asset not found.");
            }

            await _repository.DeleteAssetAsync(assetId);
        }

        public void UpdateAssetFields(AssetMetadata existingAsset, AssetMetadata updatedAsset)
        {
            if (existingAsset == null || updatedAsset == null)
            {
                throw new ArgumentNullException("Asset metadata cannot be null.");
            }

            existingAsset.Name = updatedAsset.Name;
            existingAsset.Description = updatedAsset.Description;
            existingAsset.FileFormat = updatedAsset.FileFormat;
            existingAsset.FileSize = updatedAsset.FileSize;
            existingAsset.Path = updatedAsset.Path;
            existingAsset.CreatedBy = updatedAsset.CreatedBy;
            existingAsset.VersionNumber = updatedAsset.VersionNumber;
            existingAsset.Timestamp = updatedAsset.Timestamp;
            existingAsset.UserName = updatedAsset.UserName;
            existingAsset.Comments = updatedAsset.Comments;
            existingAsset.Preview = updatedAsset.Preview;
            existingAsset.Status = updatedAsset.Status;
        }

    }
}
