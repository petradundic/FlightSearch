using FlightSearch.Server.Models;
using FlightSearch.Server.Services;
using FlightSearch.Server.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightController(FlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchFlights([FromBody] SearchParameter searchParams)
        {
            try
            {
                if (string.IsNullOrEmpty(searchParams.SearchParameterHash))
                {
                    searchParams.SearchParameterHash = HashUtility.GenerateSearchHash(searchParams);
                }
                var flightResults = await _flightService.GetFlightsBySearchParametersAsync(searchParams);
                return Ok(flightResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
    }


}

