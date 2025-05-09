using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class EventEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); //Ändrat, tidigare "public string Id { get; set; } = new Guid().ToString();"
    [Required]
    public string EventName { get; set; } = null!;
    [Required]
    public string Category { get; set; } = null!;
    public string? ImageUrl { get; set; }
    [Required]
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    [Required]
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public EventStatus Status { get; set; }

    public ICollection<MerchandiseEntity> Merchandise { get; set; } = [];
    public ICollection<PackageEntity> Packages { get; set; } = [];
    public SeatPlanEntity? SeatPlan { get; set; }

}
