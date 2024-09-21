using PlayGoodService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayGoodService.Repositories
{
    internal interface IAssetRepository
    {
        Task<IEnumerable<AssetMetadata>> GetAllAssetsAsync();
        Task<AssetMetadata> GetAssetByIdAsync(string assetId);
        Task AddAssetAsync(AssetMetadata asset);
        Task UpdateAssetAsync(AssetMetadata asset);
        Task DeleteAssetAsync(string assetId);
    }
}
