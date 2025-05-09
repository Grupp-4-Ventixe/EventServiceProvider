using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class PackageEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Event")]
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string PackageDetails { get; set; } = null!;
    public EventEntity Event { get; set; } = null!;
}
