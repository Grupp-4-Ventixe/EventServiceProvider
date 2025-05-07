using System;
using Data.Context;
using Domain.Models;
using Data.Entities;
namespace Data.Repositories;

public interface IEventRepository : IBaseRepository<EventEntity, Event>
{

}
public class EventRepository(EventDbContext context) : BaseRepository<EventEntity, Event>(context), IEventRepository
{
}