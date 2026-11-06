using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
using System.Security.Claims;
namespace ShareHour.Web.Pages.Tools;
[Authorize(Roles = "Owner")]
public class CreateModel(ShareHourDb db) : PageModel
{
    [BindProperty] public ToolItem Input { get; set; } = new();
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Input.Owner");
        ModelState.Remove("Input.Bookings");
        if (!ModelState.IsValid) return Page();
        try
        {
            Input.Id = 0;
            Input.OwnerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            db.Tools.Add(Input);
            await db.SaveChangesAsync();
            TempData["Notice"] = "Your tool is now listed.";
            return RedirectToPage("/Tools/Index");
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Could not save your listing. Please try again.");
            return Page();
        }
    }
}
