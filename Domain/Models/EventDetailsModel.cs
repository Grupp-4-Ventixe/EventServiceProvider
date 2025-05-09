namespace Domain.Models;

public class EventDetailsModel
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public DateTime StartDateTime { get; set; } 
    public string Location { get; set; } = null!;
    public decimal Price { get; set; } 
    public string Description { get; set; } = null!;
    public EventStatus Status { get; set; }

    public List<MerchandiseModel> Merchandise { get; set; } = [];
    public List<PackageModel> Packages { get; set; } = [];
    public SeatPlanModel? SeatPlan { get; set; }
}
