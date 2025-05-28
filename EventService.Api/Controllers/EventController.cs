using Microsoft.AspNetCore.Mvc;
using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Data.Entities;
using Swashbuckle.AspNetCore.Annotations;
using EventService.Api.Filters;

namespace EventService.Api.Controllers;
//Tagit hjälp av ChatGPT för SwaggerDoc
[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    /// <summary>
    /// Create a new event.
    /// </summary>
    [HttpPost]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Create a new event", Description = "Adds a new event to the event list.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventFormData model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _eventService.CreateEventAsync(model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Event added to event list successfully." : result.Error);
    }

    /// <summary>
    /// Get all events, optionally filtered by status.
    /// </summary>
    [HttpGet]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Get all events", Description = "Returns all events, optionally filtered by status.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventResponseFormData>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Get a specific event by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Get event by ID", Description = "Returns the event matching the specified ID.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventResponseFormData))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result.Error);

        var response = result.Result!.MapTo<EventResponseFormData>();
        return Ok(response);
    }

    /// <summary>
    /// Update an existing event.
    /// </summary>
    [HttpPut("{id}")]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Update an event", Description = "Updates the data of an existing event.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventResponseFormData))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Delete an event by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ApiKeyAuth]
    [SwaggerOperation(Summary = "Delete an event", Description = "Deletes an event based on the provided ID.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        return StatusCode(result.StatusCode, result.Succeeded ? "Event deleted" : result.Error);
    }
}
