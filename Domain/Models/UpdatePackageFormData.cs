namespace Domain.Models;

public class UpdatePackageFormData
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string PackageDetails { get; set; } = null!;
}
