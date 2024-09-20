
using PlayGoodAssetService.Models;
using PlayGoodAssetService.Repositories;

namespace PlayGoodAssetService.Services
{
    internal class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;

        public AssetService(IAssetRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AssetMetadata>> GetAllAssetsAsync()
        {
            return await _repository.GetAllAssetsAsync();
        }

        public async Task<AssetMetadata> GetAssetByIdAsync(string assetId)
        {
            return await _repository.GetAssetByIdAsync(assetId);
        }
    }
}
