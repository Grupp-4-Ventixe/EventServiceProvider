using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Domain.Extensions; 
using Domain.Models;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace EventService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventFormData model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _eventService.CreateEventAsync(model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Event created" : result.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] EventStatus? status)
    {
        if (status.HasValue)
        {
            var filtered = await _eventService.GetEventsByStatusAsync(status);
            var filteredResponse = filtered.Select(e => e.MapTo<EventResponseFormData>());
            return Ok(filteredResponse);
        }

        var result = await _eventService.GetAllEventsAsync();
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result.Error);

        var response = result.Result!.Select(e => e.MapTo<EventResponseFormData>());
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result.Error);

        var response = result.Result!.MapTo<EventResponseFormData>();
        return Ok(response);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventFormData model) 
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (!result.Succeeded || result.Result == null)
            return NotFound($"Event with ID '{id}' not found.");

        var domainModel = result.Result;

        var entity = new EventEntity
        {
            Id = domainModel.Id,
            EventName = model.EventName,
            Category = model.Category,
            ImageUrl = model.ImageUrl,
            StartDateTime = model.StartDateTime,
            EndDateTime = model.EndDateTime,
            Location = model.Location,
            Description = model.Description,
            Status = model.Status
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
            ImageUrl = updated.ImageUrl,
            Status = updated.Status
        };

        return Ok(response);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        return StatusCode(result.StatusCode, result.Succeeded ? "Event deleted" : result.Error);
    }
}










