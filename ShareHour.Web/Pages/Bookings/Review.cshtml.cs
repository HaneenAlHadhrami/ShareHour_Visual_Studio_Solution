using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
using System.Security.Claims;
namespace ShareHour.Web.Pages.Bookings;
[Authorize(Roles = "Owner")]
public class ReviewModel(ShareHourDb db) : PageModel
{
    public List<Booking> Pending { get; set; } = new();
    public async Task OnGetAsync()
    {
        int ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Pending = await db.Bookings.Include(b => b.Tool).Include(b => b.Renter)
            .Where(b => b.Tool.OwnerId == ownerId && b.Status == BookingStatus.Pending)
            .OrderBy(b => b.StartAt).ToListAsync();
    }
    public async Task<IActionResult> OnPostAsync(int bookingId, string decision)
    {
        int ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = await db.Bookings.Include(b => b.Tool)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.Tool.OwnerId == ownerId);
        if (booking == null) return NotFound();
        if (booking.Status != BookingStatus.Pending) { TempData["Notice"] = "This request has already been processed."; return RedirectToPage(); }
        if (decision == "Approve") booking.Status = BookingStatus.Approved;
        else if (decision == "Reject") booking.Status = BookingStatus.Rejected;
        else return BadRequest();
        await db.SaveChangesAsync();
        TempData["Notice"] = $"Request #{booking.Id} is {booking.Status}.";
        return RedirectToPage();
    }
}
