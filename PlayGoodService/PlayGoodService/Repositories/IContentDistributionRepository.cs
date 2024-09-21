using PlayGoodService.Models;

namespace PlayGoodService.Repositories
{
    public interface IContentDistributionRepository
    {
        Task<IEnumerable<ContentDistribution>> GetAllAsync();
        Task<ContentDistribution> GetByIdAsync(int id);
        Task AddAsync(ContentDistribution contentDistribution);
        Task UpdateAsync(ContentDistribution contentDistribution);
        Task DeleteAsync(int id);
    }
}
