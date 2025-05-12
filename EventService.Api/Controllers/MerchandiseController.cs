using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MerchandiseController(IMerchandiseService merchandiseService) : ControllerBase
{
    private readonly IMerchandiseService _merchandiseService = merchandiseService;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddMerchItem([FromBody] CreateMerchandiseFormData model)
    {
        var result = await _merchandiseService.CreateMerchandiseAsync(model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Merchandise added." : result.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMerch(Guid id, [FromBody] UpdateMerchandiseFormData model)
    {
        var result = await _merchandiseService.UpdateMerchandiseAsync(id, model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Updated" : result.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMerch(Guid id)
    {
        var result = await _merchandiseService.DeleteMerchandiseAsync(id);
        return StatusCode(result.StatusCode, result.Succeeded ? "Deleted" : result.Error);
    }

}
