using System.ComponentModel.DataAnnotations;
namespace ShareHour.Web.Models;
public class Booking
{
    public Booking() { }
    public Booking(int toolId, int renterId, DateTime start, DateTime end)
    { ToolId = toolId; RenterId = renterId; StartAt = start; EndAt = end; }
    public int Id { get; set; }
    public int ToolId { get; set; }
    public ToolItem Tool { get; set; } = null!;
    public int RenterId { get; set; }
    public UserAccount Renter { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    [Range(0.0, 100000)] public decimal TotalCost { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public double DurationHours() => (EndAt - StartAt).TotalHours;
}
