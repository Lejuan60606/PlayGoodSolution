using Microsoft.AspNetCore.Mvc;
using PlayGoodAssetService.Services;
using System.Net;

namespace PlayGoodAssetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            try
            {
                var assets = await _assetService.GetAllAssetsAsync();

                if (!assets.Any())
                {
                    //logger.Debug("No content.");
                    return NoContent();
                }

              //  logger.Debug("Get the asset metadata successfully!");
                return Ok(assets);

            }
            catch (Exception ex)
            {
                //logger.Error("Internal server error.");
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(string id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            return Ok(asset);
        }
    }
}
