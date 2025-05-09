using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MerchandiseEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Event")]
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public EventEntity Event { get; set; } = null!;
}
