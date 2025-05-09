using Business.Models;
using Data.Context;
using Data.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface ISeatPlanService
{
    Task<EventResult> CreateSeatPlanAsync(CreateSeatPlanFormData model);
    Task<EventResult> UpdateSeatPlanAsync(Guid eventId, UpdateSeatPlanFormData model);
    Task<EventResult> DeleteSeatPlanAsync(Guid eventId);
}

public class SeatPlanService(EventDbContext context) : ISeatPlanService
{
    private readonly EventDbContext _context = context;

    public async Task<EventResult> CreateSeatPlanAsync(CreateSeatPlanFormData model)
    {
        var exists = await _context.Events.FindAsync(model.EventId);
        if (exists == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Event not found." };

        var seatPlan = new SeatPlanEntity
        {
            Id = Guid.NewGuid(),
            EventId = model.EventId,
            Categories = model.Categories.Select(c => new SeatCategoryEntity
            {
                Id = Guid.NewGuid(),
                Name = c.Name,
                Price = c.Price,
                AvailableSeats = c.AvailableSeats
            }).ToList()
        };

        _context.SeatPlans.Add(seatPlan);
        await _context.SaveChangesAsync();

        return new EventResult { Succeeded = true, StatusCode = 201 };
    }

    //Tagit hjälp av chatGPT 4o
    public async Task<EventResult> UpdateSeatPlanAsync(Guid eventId, UpdateSeatPlanFormData model)
    {
        var plan = await _context.SeatPlans
            .Include(sp => sp.Categories)
            .FirstOrDefaultAsync(sp => sp.EventId == eventId);

        if (plan == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "SeatPlan not found." };

        if (plan.Categories.Any())
        {
            _context.SeatCategories.RemoveRange(plan.Categories);
            await _context.SaveChangesAsync(); 
        }

        var newCategories = model.Categories.Select(c => new SeatCategoryEntity
        {
            Id = Guid.NewGuid(),
            SeatPlanId = plan.Id, 
            Name = c.Name,
            Price = c.Price,
            AvailableSeats = c.AvailableSeats
        }).ToList();

        _context.SeatCategories.AddRange(newCategories);
        await _context.SaveChangesAsync(); 

        return new EventResult { Succeeded = true, StatusCode = 200 };
    }


    public async Task<EventResult> DeleteSeatPlanAsync(Guid eventId)
    {
        var plan = await _context.SeatPlans
            .Include(sp => sp.Categories)
            .FirstOrDefaultAsync(sp => sp.EventId == eventId);

        if (plan == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Not found" };

        _context.SeatCategories.RemoveRange(plan.Categories);
        _context.SeatPlans.Remove(plan);

        await _context.SaveChangesAsync();
        return new EventResult { Succeeded = true, StatusCode = 200 };
    }

}

