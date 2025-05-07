using Business.Models;
using Data.Context;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Business.Services;

public interface IEventService
{
    Task<EventResult> CreateEventAsync(CreateEventFormData dto);
    Task<EventResult<Event>> GetEventByIdAsync(Guid id);
    Task<EventResult<IEnumerable<Event>>> GetAllEventsAsync();
    Task<EventResult> DeleteEventAsync(Guid id);
    Task<EventEntity?> UpdateEventAsync(EventEntity eventEntity);
}

public class EventsService(IEventRepository repository, EventDbContext context) : IEventService
{
    private readonly IEventRepository _repository = repository;
    private readonly EventDbContext _context = context;

    public async Task<EventResult> CreateEventAsync(CreateEventFormData dto)
    {
        if (dto == null)
            return new EventResult { Succeeded = false, StatusCode = 400, Error = "Input was null." };

        var entity = dto.MapTo<EventEntity>();
        entity.Id = Guid.NewGuid(); 

        var result = await _repository.AddAsync(entity);

        return new EventResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Error = result.Error
        };
    }

    public async Task<EventResult<Event>> GetEventByIdAsync(Guid id)
    {
        var result = await _repository.GetAsync(x => x.Id == id);

        return new EventResult<Event>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Error = result.Error,
            Result = result.Result
        };
    }

    public async Task<EventResult<IEnumerable<Event>>> GetAllEventsAsync()
    {
        var result = await _repository.GetAllAsync();

        return new EventResult<IEnumerable<Event>>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Error = result.Error,
            Result = result.Result
        };
    }

    public async Task<EventEntity?> UpdateEventAsync(EventEntity eventEntity)
    {
        var exists = await _context.Events.FindAsync(eventEntity.Id);
        if (exists == null)
            return null;

        exists.EventName = eventEntity.EventName;
        exists.Category = eventEntity.Category;
        exists.StartDateTime = eventEntity.StartDateTime;
        exists.EndDateTime = eventEntity.EndDateTime;
        exists.Location = eventEntity.Location;
        exists.ImageUrl = eventEntity.ImageUrl;
        exists.Description = eventEntity.Description;

        await _context.SaveChangesAsync();
        return exists;
    }

    public async Task<EventResult> DeleteEventAsync(Guid id)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);

        if (eventEntity == null)
        {
            return new EventResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Event not found."
            };
        }

        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync();

        return new EventResult
        {
            Succeeded = true,
            StatusCode = 200
        };
    }

}















