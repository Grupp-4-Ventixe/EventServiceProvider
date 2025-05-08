using Business.Models;
using Data.Repositories;
using Domain.Models;
using Domain.Extensions;

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
        var result = await _repository.GetAsync(x => x.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new EventResult<EventDetailsModel>
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = result.Error ?? "Event not found"
            };
        }

        var model = result.Result.MapTo<EventDetailsModel>();

        return new EventResult<EventDetailsModel>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = model
        };
    }
}
