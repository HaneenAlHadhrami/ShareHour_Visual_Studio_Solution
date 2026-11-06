using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
namespace ShareHour.Web.Services;
public class BookingService(ShareHourDb db)
{
    public static decimal CalculatePrice(DateTime start, DateTime end, decimal rate)
    {
        if (end <= start || rate <= 0) throw new ArgumentException("Invalid rental period or price.");
        return Math.Round((decimal)(end - start).TotalHours * rate, 3,
            MidpointRounding.AwayFromZero);
    }
    public async Task<(Booking? booking, string? error)> RequestAsync(
        int renterId, int toolId, DateTime start, DateTime end)
    {
        if (start < DateTime.Now.AddMinutes(-1)) return (null, "Start time cannot be in the past.");
        if (end <= start) return (null, "End time must be later than start time.");
        if ((end - start).TotalDays > 7) return (null, "Maximum booking duration is seven days.");
        var tool = await db.Tools.AsNoTracking().FirstOrDefaultAsync(t => t.Id == toolId && t.IsActive);
        if (tool == null) return (null, "Tool is unavailable.");
        if (tool.OwnerId == renterId) return (null, "You cannot rent your own tool.");
        bool overlaps = await db.Bookings.AnyAsync(b => b.ToolId == toolId &&
            (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Approved) &&
            start < b.EndAt && end > b.StartAt);
        if (overlaps) return (null, "This time slot overlaps an existing booking. Choose another period.");
        var booking = new Booking { ToolId = toolId, RenterId = renterId,
            StartAt = start, EndAt = end, TotalCost = CalculatePrice(start, end, tool.HourlyPrice),
            Status = BookingStatus.Pending };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();
        return (booking, null);
    }
}
