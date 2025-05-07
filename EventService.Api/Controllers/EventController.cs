using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Domain.Extensions; 
using Domain.Models;
using Data.Entities;

namespace EventService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventFormData dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _eventService.CreateEventAsync(dto);
        return StatusCode(result.StatusCode, result.Succeeded ? "Event created" : result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _eventService.GetAllEventsAsync();
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result.Error);

        var response = result.Result!.Select(e => e.MapTo<EventResponseFormData>());
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result.Error);

        var response = result.Result!.MapTo<EventResponseFormData>();
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventFormData dto)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (!result.Succeeded || result.Result == null)
            return NotFound($"Event with ID '{id}' not found.");

        var domainModel = result.Result;

        var entity = new EventEntity
        {
            Id = domainModel.Id,
            EventName = dto.EventName,
            Category = dto.Category,
            ImageUrl = dto.ImageUrl,
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            Location = dto.Location,
            Description = dto.Description
        };

        var updated = await _eventService.UpdateEventAsync(entity);
        if (updated == null)
            return StatusCode(500, "Unexpected error while updating event.");

        var response = new EventResponseFormData
        {
            Id = updated.Id,
            EventName = updated.EventName,
            Category = updated.Category,
            StartDateTime = updated.StartDateTime,
            Location = updated.Location,
            Description = updated.Description,
            ImageUrl = updated.ImageUrl
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        return StatusCode(result.StatusCode, result.Succeeded ? "Event deleted" : result.Error);
    }
}










