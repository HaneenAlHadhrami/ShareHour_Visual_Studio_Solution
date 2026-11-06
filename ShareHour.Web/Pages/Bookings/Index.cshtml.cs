using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
using System.Security.Claims;
namespace ShareHour.Web.Pages.Bookings;
[Authorize]
public class IndexModel(ShareHourDb db) : PageModel
{
    public List<Booking> Items { get; set; } = new();
    public BookingStatus? Status { get; set; }
    public async Task OnGetAsync(BookingStatus? status)
    {
        Status = status;
        int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var q = db.Bookings.Include(b => b.Tool).Where(b => b.RenterId == id);
        if (status.HasValue) q = q.Where(b => b.Status == status.Value);
        Items = await q.OrderByDescending(b => b.CreatedAt).ToListAsync();
    }
    public async Task<IActionResult> OnPostCancelAsync(int id)
    {
        int renterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.RenterId == renterId);
        if (booking == null) return NotFound();
        if (booking.Status != BookingStatus.Pending)
        {
            TempData["Notice"] = "Only pending requests can be cancelled here.";
            return RedirectToPage();
        }
        booking.Status = BookingStatus.Cancelled;
        await db.SaveChangesAsync();
        TempData["Notice"] = "Request cancelled.";
        return RedirectToPage();
    }
}
