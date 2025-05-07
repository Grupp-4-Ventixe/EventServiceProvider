using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Data.Context;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
{
    public DbSet<EventEntity> Events { get; set; }
}
