
namespace Domain.Models;

public class UpdateEventFormData
{
    public string EventName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public EventStatus Status { get; set; }



}
