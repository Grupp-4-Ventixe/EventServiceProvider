namespace Domain.Models;

public class SeatCategoryModel
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
}
