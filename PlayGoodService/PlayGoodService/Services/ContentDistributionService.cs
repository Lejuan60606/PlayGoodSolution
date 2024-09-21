using PlayGoodService.Models;
using PlayGoodService.Repositories;

namespace PlayGoodService.Services
{
    public class ContentDistributionService : IContentDistributionService
    {
        private readonly IContentDistributionRepository _repository;

        public ContentDistributionService(IContentDistributionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContentDistribution>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ContentDistribution> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(ContentDistribution contentDistribution)
        {
            await _repository.AddAsync(contentDistribution);
        }

        public async Task UpdateAsync(int id, ContentDistribution contentDistribution)
        {
            if (id != contentDistribution.Id)
            {
                throw new ArgumentException("ID mismatch.");
            }

            await _repository.UpdateAsync(contentDistribution);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
