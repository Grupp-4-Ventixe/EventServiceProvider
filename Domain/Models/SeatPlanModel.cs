namespace Domain.Models;

public class SeatPlanModel
{
    public Guid Id { get; set; }
    public List<SeatCategoryModel> Categories { get; set; } = [];
}
