using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGoodService.Models;
using PlayGoodService.Services;

namespace PlayGoodService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BriefingMetadataController : ControllerBase
    {
        private readonly IBriefingService _BriefingService;
        private readonly ILogger<BriefingMetadataController> _logger;

        public BriefingMetadataController(IBriefingService BriefingService, ILogger<BriefingMetadataController> logger)
        {
            _BriefingService = BriefingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBriefingMetadata()
        {
            try
            {
                _logger.LogInformation("Fetching all Briefing metadata.");
                var Briefings = await _BriefingService.GetAllBriefingsAsync();

                if (!Briefings.Any())
                {
                    _logger.LogInformation("No content.");
                    return NoContent();
                }

                _logger.LogInformation("Fetching all Briefing metadata succssfully.");
                return Ok(Briefings);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBriefingById(string id)
        {
            _logger.LogInformation($"Fetching Briefing {id}");
            try
            {
                var Briefing = await _BriefingService.GetBriefingByIdAsync(id);
                if (Briefing == null)
                {
                    _logger.LogWarning($"Briefing metadata for {id} not found.");
                    return NotFound();
                }
                return Ok(Briefing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBriefing([FromBody] BriefingMetadata BriefingMetadata)
        {
            if (BriefingMetadata == null)
            {
                _logger.LogWarning("BriefingMetadata object is null in adding Briefing.");
                return BadRequest("BriefingMetadata object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Received invalid Briefing object in adding Briefing.");
                return BadRequest("Invalid model object.");
            }

            try
            {
                await _BriefingService.AddBriefingAsync(BriefingMetadata);
                return CreatedAtAction(nameof(GetBriefingById), new { id = BriefingMetadata.AssetId }, BriefingMetadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Briefing.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBriefing(string id, [FromBody] BriefingMetadata BriefingMetadata)
        {
            if (BriefingMetadata == null)
            {
                _logger.LogWarning("Received null BriefingMetadata object in Update.");
                return BadRequest("BriefingMetadata object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Received invalid BriefingMetadata object in Update.");
                return BadRequest("Invalid model object.");
            }

            try
            {
                await _BriefingService.UpdateBriefingAsync(id, BriefingMetadata);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating Briefing {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBriefing(string id)
        {
            _logger.LogInformation($"Attempting to delete Briefing: {id}");
            try
            {
                await _BriefingService.DeleteBriefingAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting Briefing: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
