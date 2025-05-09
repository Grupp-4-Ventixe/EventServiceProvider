using System;
using Data.Context;
using Domain.Models;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
namespace Data.Repositories;

public interface IEventRepository : IBaseRepository<EventEntity, Event>
{
    Task<RepositoryResult<EventEntity>> GetEntityWithDetailsAsync(Guid id);
}
public class EventRepository : BaseRepository<EventEntity, Event>, IEventRepository
{
    public EventRepository(EventDbContext context) : base(context) { }

    public async Task<RepositoryResult<EventEntity>> GetEntityWithDetailsAsync(Guid id)
    {
        try
        {
            var entity = await _context.Events
                .Include(e => e.Merchandise)
                .Include(e => e.Packages)
                .Include(e => e.SeatPlan)
                    .ThenInclude(sp => sp!.Categories)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                return new RepositoryResult<EventEntity> { Succeeded = false, StatusCode = 404, Error = "Event not found." };

            return new RepositoryResult<EventEntity> { Succeeded = true, StatusCode = 200, Result = entity };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<EventEntity> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
}
