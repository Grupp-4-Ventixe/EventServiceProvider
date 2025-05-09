using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models;

public class CreateSeatPlanFormData
{
    public Guid EventId { get; set; }
    public List<SeatCategoryModel> Categories { get; set; } = [];
}
