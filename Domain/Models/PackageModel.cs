namespace Domain.Models;

public class PackageModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string PackageDetails { get; set; } = null!;
}
