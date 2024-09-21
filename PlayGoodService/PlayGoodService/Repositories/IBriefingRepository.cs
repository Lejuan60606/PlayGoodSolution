using PlayGoodService.Models;

namespace PlayGoodBriefingService.Repositories
{
    internal interface IBriefingRepository
    {
        Task<IEnumerable<BriefingMetadata>> GetAllBriefingsAsync();
        Task<BriefingMetadata> GetBriefingByIdAsync(string BriefingId);
        Task AddBriefingAsync(BriefingMetadata Briefing);
        Task UpdateBriefingAsync(BriefingMetadata Briefing);
        Task DeleteBriefingAsync(string BriefingId);
    }
}
