using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlayGoodService.Models
{
    public class ContentDistribution
    {
        [Key]
        public int Id { get; set; }

        public string? DistributionDate { get; set; }

        [NotMapped]
        public List<string> DistributionChannels { get; set; }

        [NotMapped]
        public List<string> DistributionMethods { get; set; }

        public string DistributionChannelsSerialized
        {
            get => DistributionChannels != null ? string.Join(",", DistributionChannels) : null; //serialize to strings for storage
            set => DistributionChannels = value?.Split(',').ToList();
        }

        public string DistributionMethodsSerialized
        {
            get => DistributionMethods != null ? string.Join(",", DistributionMethods) : null; //serialize to strings for storage
            set => DistributionMethods = value?.Split(',').ToList();
        }

        public ICollection<Asset> Assets { get; set; }

    }
}
