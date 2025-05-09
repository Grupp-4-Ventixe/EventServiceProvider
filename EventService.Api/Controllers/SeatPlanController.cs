using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatPlanController(ISeatPlanService seatPlanService) : ControllerBase
{
    private readonly ISeatPlanService _seatPlanService = seatPlanService;

    [HttpPost]
    public async Task<IActionResult> AddSeatPlan([FromBody] CreateSeatPlanFormData model)
    {
        var result = await _seatPlanService.CreateSeatPlanAsync(model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Seat plan added." : result.Error);
    }

    [HttpPut("{eventId}")]
    public async Task<IActionResult> UpdateSeatPlan(Guid eventId, [FromBody] UpdateSeatPlanFormData model)
    {
        var result = await _seatPlanService.UpdateSeatPlanAsync(eventId, model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Updated" : result.Error);
    }

    [HttpDelete("{eventId}")]
    public async Task<IActionResult> DeleteSeatPlan(Guid eventId)
    {
        var result = await _seatPlanService.DeleteSeatPlanAsync(eventId);
        return StatusCode(result.StatusCode, result.Succeeded ? "Deleted" : result.Error);
    }

}