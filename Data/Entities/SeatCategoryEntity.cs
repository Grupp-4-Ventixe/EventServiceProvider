using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class SeatCategoryEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("SeatPlan")]
    public Guid SeatPlanId { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }

    public SeatPlanEntity SeatPlan { get; set; } = null!;
}
