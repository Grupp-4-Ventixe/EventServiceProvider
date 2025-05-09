using Data.Context;
using Data.Entities;
using Business.Models;
using Domain.Models;

namespace Business.Services;

public interface IPackageService
{
    Task<EventResult> CreatePackageAsync(CreatePackageFormData model);
    Task<EventResult> UpdatePackageAsync(Guid id, UpdatePackageFormData model);
    Task<EventResult> DeletePackageAsync(Guid id);
}

public class PackageService(EventDbContext context) : IPackageService
{
    private readonly EventDbContext _context = context;

    public async Task<EventResult> CreatePackageAsync(CreatePackageFormData model)
    {
        var exists = await _context.Events.FindAsync(model.EventId);
        if (exists == null)
        {
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Event not found." };
        }

        var package = new PackageEntity
        {
            Id = Guid.NewGuid(),
            EventId = model.EventId,
            Name = model.Name,
            Price = model.Price,
            PackageDetails = model.PackageDetails
        };

        _context.Packages.Add(package);
        await _context.SaveChangesAsync();

        return new EventResult { Succeeded = true, StatusCode = 201 };
    }

    public async Task<EventResult> UpdatePackageAsync(Guid id, UpdatePackageFormData model)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Not found" };

        package.Name = model.Name;
        package.Price = model.Price;
        package.PackageDetails = model.PackageDetails;

        await _context.SaveChangesAsync();
        return new EventResult { Succeeded = true, StatusCode = 200 };
    }

    public async Task<EventResult> DeletePackageAsync(Guid id)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null)
            return new EventResult { Succeeded = false, StatusCode = 404, Error = "Not found" };

        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();
        return new EventResult { Succeeded = true, StatusCode = 200 };
    }

}
