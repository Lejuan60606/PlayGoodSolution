
using PlayGoodBriefingService.Repositories;
using PlayGoodService.Models;
using PlayGoodService.Repositories;

namespace PlayGoodService.Services
{
    internal class BriefingService : IBriefingService
    {
        private readonly IBriefingRepository _repository;
        private readonly ILogger<BriefingService> _logger;

        public BriefingService(IBriefingRepository repository, ILogger<BriefingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<BriefingMetadata>> GetAllBriefingsAsync()
        {
            _logger.LogDebug("Getting all Briefings from repository.");
            return await _repository.GetAllBriefingsAsync();
        }

        public async Task<BriefingMetadata> GetBriefingByIdAsync(string BriefingId)
        {
            _logger.LogDebug($"Getting Briefing {BriefingId} from repository.");
            return await _repository.GetBriefingByIdAsync(BriefingId);
        }

        public async Task AddBriefingAsync(BriefingMetadata Briefing)
        {
            _logger.LogInformation($"Adding new Briefing {Briefing.AssetId}");
            await _repository.AddBriefingAsync(Briefing);
        }

        public async Task UpdateBriefingAsync(string BriefingId, BriefingMetadata Briefing)
        {
            if (BriefingId != Briefing.AssetId)
            {
                _logger.LogWarning("Briefing ID mismatch in update operation.");
                throw new ArgumentException("Briefing ID mismatch.");
            }

            var existingBriefing = await _repository.GetBriefingByIdAsync(BriefingId);
            if (existingBriefing == null)
            {
                _logger.LogWarning($"Briefing {BriefingId} not found for update.");
                throw new KeyNotFoundException("Briefing not found.");
            }

            _logger.LogInformation($"Updating Briefing {BriefingId}");
            await _repository.UpdateBriefingAsync(Briefing);
        }

        public async Task DeleteBriefingAsync(string BriefingId)
        {
            _logger.LogInformation($"Deleting Briefing {BriefingId}");

            var existingBriefing =  await _repository.GetBriefingByIdAsync(BriefingId);

            if (existingBriefing == null)
            {
                _logger.LogWarning($"Briefing {BriefingId} not found for update.");
                throw new KeyNotFoundException("Briefing not found.");
            }

            await _repository.DeleteBriefingAsync(BriefingId);
        }
    }
}
