using Newtonsoft.Json;
using PlayGoodAssetService.Models;

namespace PlayGoodAssetService.Data
{
    internal class DbInitializer
    {
        public static void Initialize(AssetAppDbContext context)
        {
            if (context.AssetMetadata.Any())
            {
                return; 
            }

            var assetData = File.ReadAllText("Data/assetMetadata.json");
            var assets = JsonConvert.DeserializeObject<List<AssetMetadata>>(assetData);
            context.AssetMetadata.AddRange(assets);

            //var briefData = File.ReadAllText("Data/briefMetadata.json");
            //var briefs = JsonConvert.DeserializeObject<List<BriefMetadata>>(briefData);
            //context.Briefs.AddRange(briefs);

            //var contentDistributionData = File.ReadAllText("Data/contentDistributionMetadata.json");
            //var contentDistributions = JsonConvert.DeserializeObject<ContentDistributionMetadata>(contentDistributionData);
            //context.ContentDistributions.Add(contentDistributions);

            context.SaveChanges();

            context.SaveChanges();
        }
    }
}
