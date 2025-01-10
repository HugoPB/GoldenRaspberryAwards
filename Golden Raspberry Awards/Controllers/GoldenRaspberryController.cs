using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Golden_Raspberry_Awards.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoldenRaspberryController(IGoldenRaspberryService goldenRaspberryService
                                            ,ILogger<GoldenRaspberryController> logger) : ControllerBase
    {
        private readonly IGoldenRaspberryService _goldenRaspberryService = goldenRaspberryService;
        private readonly ILogger<GoldenRaspberryController> _logger = logger;

        [HttpGet]
        [Route("GetAllAwards")]
        public async Task<IActionResult> GetAllAwards()
        {
            try
            {
                var allAwards = await _goldenRaspberryService.GetAllAwards();

                return Ok(allAwards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetWinnersInterval")]
        public async Task<IActionResult> GetMinMaxWinnersInterval()
        {
            try
            {
                var allAwards = await _goldenRaspberryService.GetMinMaxWinnersInterval();

                return Ok(allAwards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }
    }
}
