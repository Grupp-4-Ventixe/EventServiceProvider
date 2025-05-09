namespace Domain.Models;

public class CreatePackageFormData
{
    public Guid EventId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string PackageDetails { get; set; } = null!;
}
