using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGoodService.Models;
using PlayGoodService.Services;

namespace PlayGoodService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetService assetService, ILogger<AssetsController> logger)
        {
            _assetService = assetService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssetMetadata()
        {
            try
            {
                _logger.LogInformation("Fetching all asset metadata.");
                var assets = await _assetService.GetAllAssetsAsync();

                if (!assets.Any())
                {
                    _logger.LogInformation("No content.");
                    return NoContent();
                }

                _logger.LogInformation("Fetching all asset metadata succssfully.");
                return Ok(assets);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(string id)
        {
            _logger.LogInformation($"Fetching asset {id}");
            try
            {
                var asset = await _assetService.GetAssetByIdAsync(id);
                if (asset == null)
                {
                    _logger.LogWarning($"Asset metadata for {id} not found.");
                    return NotFound();
                }
                return Ok(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsset([FromBody] AssetMetadata assetMetadata)
        {
            if (assetMetadata == null)
            {
                _logger.LogWarning("assetMetadata object is null in adding asset.");
                return BadRequest("assetMetadata object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Received invalid asset object in adding asset.");
                return BadRequest("Invalid model object.");
            }

            try
            {
                await _assetService.AddAssetAsync(assetMetadata);
                return CreatedAtAction(nameof(GetAssetById), new { id = assetMetadata.AssetId }, assetMetadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating asset.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(string id, [FromBody] AssetMetadata assetMetadata)
        {
            if (assetMetadata == null)
            {
                _logger.LogWarning("Received null assetMetadata object in Update.");
                return BadRequest("assetMetadata object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Received invalid assetMetadata object in Update.");
                return BadRequest("Invalid model object.");
            }

            try
            {
                await _assetService.UpdateAssetAsync(id, assetMetadata);
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
                _logger.LogError(ex, $"Error occurred while updating asset {id}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(string id)
        {
            _logger.LogInformation($"Attempting to delete asset with ID: {id}");
            try
            {
                await _assetService.DeleteAssetAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting asset with ID: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
