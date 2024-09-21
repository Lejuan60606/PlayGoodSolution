using System.ComponentModel.DataAnnotations;

namespace PlayGoodAssetService.Models
{
    public class AssetMetadata
    {
        [Key]
        [Required]
        public string AssetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileFormat { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public string CreatedBy { get; set; }
        public string VersionNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string Comments { get; set; }
        public string Preview { get; set; }
        public string Status { get; set; }
    }
}
