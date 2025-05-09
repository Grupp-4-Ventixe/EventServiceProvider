namespace Domain.Models;

public class CreateMerchandiseFormData
{
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
