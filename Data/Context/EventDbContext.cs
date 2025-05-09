using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Data.Context;

public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
{
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<MerchandiseEntity> Merchandise { get; set; }
    public DbSet<PackageEntity> Packages { get; set; }
    public DbSet<SeatPlanEntity> SeatPlans { get; set; }
    public DbSet<SeatCategoryEntity> SeatCategories { get; set; }
}
