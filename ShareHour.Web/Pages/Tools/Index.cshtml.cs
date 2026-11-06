using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
namespace ShareHour.Web.Pages.Tools;
public class IndexModel(ShareHourDb db) : PageModel
{
    public List<ToolItem> Items { get; set; } = new();
    public string Category { get; set; } = "All";
    public async Task OnGetAsync(string? category)
    {
        Category = category ?? "All";
        var q = db.Tools.Include(t => t.Owner).Where(t => t.IsActive).AsQueryable();
        if (Enum.TryParse<ToolCategory>(Category, out var selected))
            q = q.Where(t => t.Category == selected);
        Items = await q.OrderBy(t => t.Name).ToListAsync();
    }
}
