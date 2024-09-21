
using System.ComponentModel.DataAnnotations;

namespace PlayGoodService.Models
{
    public class BriefingMetadata
    {
        [Key]
        [Required]
        public string AssetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }     
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comments { get; set; }       
        public string Status { get; set; }
    }
}
