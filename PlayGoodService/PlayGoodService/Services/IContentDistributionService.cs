using PlayGoodService.Models;

namespace PlayGoodService.Services
{
    public interface IContentDistributionService
    {
        Task<IEnumerable<ContentDistribution>> GetAllAsync();
        Task<ContentDistribution> GetByIdAsync(int id);
        Task AddAsync(ContentDistribution contentDistribution);
        Task UpdateAsync(int id, ContentDistribution contentDistribution);
        Task DeleteAsync(int id);
    }
}
