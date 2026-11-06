using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
namespace ShareHour.Web.Pages;
public class IndexModel(ShareHourDb db) : PageModel
{
    public List<ToolItem> Tools { get; set; } = new();
    public string Search { get; set; } = "";
    public async Task OnGetAsync(string? search)
    {
        Search = search?.Trim() ?? "";
        var query = db.Tools.Include(t => t.Owner).Where(t => t.IsActive).AsQueryable();
        if (!string.IsNullOrWhiteSpace(Search))
            query = query.Where(t => t.Name.Contains(Search) || t.Description.Contains(Search));
        Tools = await query.OrderBy(t => t.Name).ToListAsync();
    }
}
