using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities;

public class SeatPlanEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("Event")]
    public Guid EventId { get; set; }

    public EventEntity Event { get; set; } = null!;
    public ICollection<SeatCategoryEntity> Categories { get; set; } = [];
}
