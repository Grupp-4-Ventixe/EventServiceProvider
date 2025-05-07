namespace Domain.Models;

public class EventResponseFormData
{
    public Guid Id { get; set; } //Ändrat, tidigare "public string Id { get; set; } = new Guid().ToString();"
    public string EventName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime StartDateTime { get; set; }
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
}
