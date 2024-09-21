namespace PlayGoodService.Models
{
    public class Asset
    {
        public string AssetId { get; set; }
        public string Name { get; set; }
        public string FileURL { get; set; }

        public int ContentDistributionId { get; set; }
        public ContentDistribution ContentDistribution { get; set; }
    }
}
