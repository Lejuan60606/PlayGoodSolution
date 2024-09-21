using Newtonsoft.Json;
using PlayGoodService.Models;

namespace PlayGoodService.Data
{
    internal class DbInitializer
    {
        public static void Initialize(AssetAppDbContext context)
        {
            if (!context.AssetMetadatas.Any())
            {
                try
                {
                    var assetData = File.ReadAllText("Sample/assetMetadata.json");
                    var assets = JsonConvert.DeserializeObject<List<AssetMetadata>>(assetData);

                    if (assets != null && assets.Any())
                    {
                        context.AssetMetadatas.AddRange(assets);
                        Console.WriteLine("AssetMetadatas seeded successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding AssetMetadatas: {ex.Message}");
                }
            }

            if (!context.BriefingMetadatas.Any())
            {
                try
                {
                    var briefData = File.ReadAllText("Sample/briefMetadata.json");
                    var briefs = JsonConvert.DeserializeObject<List<BriefingMetadata>>(briefData);

                    if (briefs != null && briefs.Any())
                    {
                        context.BriefingMetadatas.AddRange(briefs);
                        Console.WriteLine("BriefingMetadatas seeded successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding BriefingMetadatas: {ex.Message}");
                }
            }

            if (!context.ContentDistributionMetadatas.Any())
            {
                try
                {
                    var contentDistributionData = File.ReadAllText("Sample/contentDistributionMetadata.json");
                    var contentDistribution = JsonConvert.DeserializeObject<List<ContentDistribution>>(contentDistributionData);

                    if (contentDistribution != null)
                    {
                        context.ContentDistributionMetadatas.AddRange(contentDistribution);
                        Console.WriteLine("ContentDistributionMetadata seeded successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding ContentDistributionMetadatas: {ex.Message}");
                }
            }

            context.SaveChanges();

        }
    }
}
