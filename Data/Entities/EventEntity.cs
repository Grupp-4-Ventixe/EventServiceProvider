namespace Data.Entities;

public class EventEntity
{
    public Guid Id { get; set; } = Guid.NewGuid(); //Ändrat, tidigare "public string Id { get; set; } = new Guid().ToString();"
    public string EventName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? ImageUrl { get; set; } 
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;
   
}
