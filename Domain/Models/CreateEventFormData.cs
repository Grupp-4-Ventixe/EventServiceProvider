using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class CreateEventFormData
{
    [Required]
    public string EventName { get; set; } = null!;

    [Required]
    public string Category { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }

    [Required]
    public string Location { get; set; } = null!;

    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Draft;

}
