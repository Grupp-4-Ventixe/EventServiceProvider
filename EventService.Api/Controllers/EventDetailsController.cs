using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace EventService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventDetailsController(IEventDetailsService eventDetailsService) : ControllerBase
    {
        private readonly IEventDetailsService _eventDetailsService = eventDetailsService;

        [Authorize]
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetEventDetails(Guid id)
        {
            var result = await _eventDetailsService.GetEventDetailsByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
                return StatusCode(result.StatusCode, result.Error);

            return Ok(result.Result);
        }
    }
}
