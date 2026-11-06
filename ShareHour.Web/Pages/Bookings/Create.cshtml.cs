using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
using ShareHour.Web.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace ShareHour.Web.Pages.Bookings;
[Authorize(Roles = "Renter")]
public class CreateModel(ShareHourDb db, BookingService bookingService) : PageModel
{
    public ToolItem? Tool { get; set; }
    [BindProperty] public BookingInput Input { get; set; } = new();
    public class BookingInput
    {
        [Range(1, int.MaxValue)] public int ToolId { get; set; }
        [Required, DataType(DataType.DateTime)] public DateTime StartAt { get; set; }
        [Required, DataType(DataType.DateTime)] public DateTime EndAt { get; set; }
        [Required] public bool AcceptTerms { get; set; }
    }
    public async Task<IActionResult> OnGetAsync(int toolId)
    {
        Tool = await db.Tools.Include(t => t.Owner).FirstOrDefaultAsync(t => t.Id == toolId && t.IsActive);
        if (Tool == null) return NotFound();
        Input.ToolId = toolId;
        Input.StartAt = DateTime.Now.AddHours(1);
        Input.EndAt = DateTime.Now.AddHours(2);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Tool = await db.Tools.Include(t => t.Owner).FirstOrDefaultAsync(t => t.Id == Input.ToolId && t.IsActive);
        if (Tool == null) return NotFound();
        if (!Input.AcceptTerms) ModelState.AddModelError("Input.AcceptTerms", "Please confirm the booking conditions.");
        if (!ModelState.IsValid) return Page();
        try
        {
            int renterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await bookingService.RequestAsync(renterId, Input.ToolId, Input.StartAt, Input.EndAt);
            if (result.error != null)
            {
                ModelState.AddModelError("", result.error);
                return Page();
            }
            TempData["Notice"] = $"Booking #{result.booking!.Id} submitted as Pending. Owner approval is required.";
            return RedirectToPage("/Bookings/Index");
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "We could not submit the booking. No confirmation has been issued.");
            return Page();
        }
    }
}
