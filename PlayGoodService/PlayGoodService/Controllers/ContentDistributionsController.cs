
using Microsoft.AspNetCore.Mvc;
using PlayGoodService.Models;
using PlayGoodService.Services;

namespace PlayGoodService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentDistributionsController : ControllerBase
    {
        private readonly IContentDistributionService _service;
        private readonly ILogger<ContentDistributionsController> _logger;

        public ContentDistributionsController(IContentDistributionService service, ILogger<ContentDistributionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var distributions = await _service.GetAllAsync();
            if (!distributions.Any())
            {
                _logger.LogInformation("No content.");
                return NoContent();
            }

            _logger.LogInformation("Fetching all distribution metadata succssfully.");
            return Ok(distributions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var distribution = await _service.GetByIdAsync(id);
                if (distribution == null)
                {
                    _logger.LogWarning($"distribution metadata for {id} not found.");
                    return NotFound();
                }
                return Ok(distribution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error.");
                return StatusCode(500, "Internal server error.");
            }        
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContentDistribution contentDistribution)
        {
            if (contentDistribution == null)
            {
                _logger.LogWarning("distribution Metadata object is null.");
                return BadRequest("distribution Metadata object is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.AddAsync(contentDistribution);
                return CreatedAtAction(nameof(GetById), new { id = contentDistribution.Id }, contentDistribution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating asset.");
                return StatusCode(500, "Internal server error.");
            }           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContentDistribution contentDistribution)
        {
            if (contentDistribution == null)
            {
                _logger.LogWarning("Received null contentDistribution object in Update.");
                return BadRequest("contentDistribution object is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Received invalid contentDistribution object in Update.");
                return BadRequest("Invalid model object.");
            }

            try
            {
                await _service.UpdateAsync(id, contentDistribution);
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
                _logger.LogError(ex, $"Error occurred while updating {id}");
                return StatusCode(500, "Internal server error.");
            }           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Attempting to delete distribution: {id}");
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting: {id}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
