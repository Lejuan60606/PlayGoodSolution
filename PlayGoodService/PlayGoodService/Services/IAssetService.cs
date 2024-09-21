using PlayGoodAssetService.Models;

namespace PlayGoodAssetService.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetMetadata>> GetAllAssetsAsync();
        Task<AssetMetadata> GetAssetByIdAsync(string assetId);
        Task AddAssetAsync(AssetMetadata asset);
        Task UpdateAssetAsync(string assetId, AssetMetadata asset);
        Task DeleteAssetAsync(string assetId);
    }
}
