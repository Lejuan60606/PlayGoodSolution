using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlayGoodService.Models
{
    public class Asset
    {
        public string AssetId { get; set; }
        public string Name { get; set; }
        public string FileURL { get; set; }

        public int ContentDistributionId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ContentDistribution? ContentDistribution { get; set; }
    }
}
