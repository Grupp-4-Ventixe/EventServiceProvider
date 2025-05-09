using Business.Models;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public interface IEventDetailsService
{
    Task<EventResult<EventDetailsModel>> GetEventDetailsByIdAsync(Guid id);
}

public class EventDetailsService(IEventRepository repository) : IEventDetailsService
{
    private readonly IEventRepository _repository = repository;

    public async Task<EventResult<EventDetailsModel>> GetEventDetailsByIdAsync(Guid id)
    {
        var result = await _repository.GetEntityWithDetailsAsync(id);

        if (!result.Succeeded || result.Result == null)
        {
            return new EventResult<EventDetailsModel>
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = result.Error ?? "Event not found"
            };
        }

        var entity = result.Result;

        var model = new EventDetailsModel
        {
            Id = entity.Id,
            EventName = entity.EventName,
            Category = entity.Category,
            StartDateTime = entity.StartDateTime,
            Location = entity.Location,
            Price = entity.Price,
            Description = entity.Description,
            Status = entity.Status,
            Merchandise = entity.Merchandise.Select(m => new MerchandiseModel
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                ImageUrl = m.ImageUrl
            }).ToList(),
            Packages = entity.Packages.Select(p => new PackageModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                PackageDetails = p.PackageDetails
            }).ToList(),
            SeatPlan = entity.SeatPlan == null ? null : new SeatPlanModel
            {
                Id = entity.SeatPlan.Id,
                Categories = entity.SeatPlan.Categories.Select(c => new SeatCategoryModel
                {
                    Name = c.Name,
                    Price = c.Price,
                    AvailableSeats = c.AvailableSeats
                }).ToList()
            }
        };

        return new EventResult<EventDetailsModel>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = model
        };
    }

}
