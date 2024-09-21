using Microsoft.EntityFrameworkCore;
using PlayGoodService.Data;
using PlayGoodService.Models;

namespace PlayGoodBriefingService.Repositories
{
    internal class BriefingRepository : IBriefingRepository
    {
        private readonly AssetAppDbContext _context;
        private readonly ILogger<BriefingRepository> _logger;

        public BriefingRepository(AssetAppDbContext context, ILogger<BriefingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<BriefingMetadata>> GetAllBriefingsAsync()
        {

            try
            {
                _logger.LogInformation("Fetching all Briefings from the database.");
                var BriefingsMetadata = await _context.BriefingMetadatas.ToListAsync();
                _logger.LogInformation($"Successfully retrieved {BriefingsMetadata.Count} Briefings metadata.", BriefingsMetadata.Count);
                return BriefingsMetadata;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateBriefingAsync.");
                throw new Exception("Database update error.", ex);
            }
        }

        public async Task<BriefingMetadata> GetBriefingByIdAsync(string BriefingMetadataId)
        {
            try
            {
                _logger.LogInformation($"Fetching Briefing {BriefingMetadataId}.");
                var BriefingMetadata = await _context.BriefingMetadatas.FindAsync(BriefingMetadataId);

                if (BriefingMetadata == null)
                {
                    _logger.LogWarning($"Briefing metadata {BriefingMetadataId} not found.");
                }
                else
                {
                    _logger.LogInformation($"Successfully retrieved Briefing medatada {BriefingMetadataId}.");
                }

                return BriefingMetadata;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateBriefingAsync.");
                throw new Exception("Database update error.", ex);
            }           
        }

        public async Task AddBriefingAsync(BriefingMetadata BriefingMetadata)
        {
            try
            {
                _logger.LogInformation($"Adding a new Briefing {BriefingMetadata.AssetId} to the database.");
                await _context.BriefingMetadatas.AddAsync(BriefingMetadata);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully added Briefing metadata {BriefingMetadata.AssetId}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateBriefingAsync.");
                throw new Exception("Database update error.", ex);
            }          
        }

        public async Task UpdateBriefingAsync(BriefingMetadata BriefingMetadata)
        {
            try
            {
                _logger.LogInformation($"Updating Briefing with ID {BriefingMetadata.AssetId}.");
                _context.BriefingMetadatas.Update(BriefingMetadata);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated Briefing with ID {BriefingMetadata.AssetId}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in UpdateBriefingAsync.");
                throw new Exception("Database update error.", ex);
            }         
        }

        public async Task DeleteBriefingAsync(string BriefingId)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete Briefing {BriefingId} metadata.");
                var BriefingMetadata = await _context.BriefingMetadatas.FindAsync(BriefingId);

                if (BriefingMetadata != null)
                {
                    _context.BriefingMetadatas.Remove(BriefingMetadata);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Attempting to delete Briefing {BriefingId} metadata.");
                }
                else
                {
                    _logger.LogWarning($"Attempting to delete Briefing {BriefingId} metadata.");
                    throw new KeyNotFoundException("Briefing not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in DeleteBriefingAsync.");
                throw new Exception("Database update error.", ex);
            }          
        }
    }

}
