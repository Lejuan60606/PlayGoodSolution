using PlayGoodService.Models;

namespace PlayGoodService.Services
{
    public interface IBriefingService
    {
        Task<IEnumerable<BriefingMetadata>> GetAllBriefingsAsync();
        Task<BriefingMetadata> GetBriefingByIdAsync(string BriefingId);
        Task AddBriefingAsync(BriefingMetadata Briefing);
        Task UpdateBriefingAsync(string BriefingId, BriefingMetadata Briefing);
        Task DeleteBriefingAsync(string BriefingId);
    }
}
