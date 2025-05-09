using Data.Context;
using Data.Entities;
using Business.Models;
using Domain.Models;

namespace Business.Services;

public interface IMerchandiseService
{
    Task<EventResult> CreateMerchandiseAsync(CreateMerchandiseFormData model);
    Task<EventResult> UpdateMerchandiseAsync(Guid id, UpdateMerchandiseFormData model);
    Task<EventResult> DeleteMerchandiseAsync(Guid id);
}

public class MerchandiseService(EventDbContext context) : IMerchandiseService
{
    private readonly EventDbContext _context = context;

    public async Task<EventResult> CreateMerchandiseAsync(CreateMerchandiseFormData model)
    {
        var eventExists = await _context.Events.FindAsync(model.EventId);
        if (eventExists == null)
        {
            return new EventResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Event not found."
            };
        }

        var merch = new MerchandiseEntity
        {
            Id = Guid.NewGuid(),
            EventId = model.EventId,
            Name = model.Name,
            Price = model.Price,
            ImageUrl = model.ImageUrl
        };

        _context.Merchandise.Add(merch);
        await _context.SaveChangesAsync();

        return new EventResult
        {
            Succeeded = true,
            StatusCode = 201
        };
    }

    public async Task<EventResult> UpdateMerchandiseAsync(Guid id, UpdateMerchandiseFormData model)
    {
        var merch = await _context.Merchandise.FindAsync(id);
        if (merch == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Item not found." };

        merch.Name = model.Name;
        merch.Price = model.Price;
        merch.ImageUrl = model.ImageUrl;

        await _context.SaveChangesAsync();

        return new EventResult { Succeeded = true, StatusCode = 200 };
    }

    public async Task<EventResult> DeleteMerchandiseAsync(Guid id)
    {
        var merch = await _context.Merchandise.FindAsync(id);
        if (merch == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Item not found." };

        _context.Merchandise.Remove(merch);
        await _context.SaveChangesAsync();

        return new EventResult { Succeeded = true, StatusCode = 200 };
    }

}


