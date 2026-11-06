using System.ComponentModel.DataAnnotations;
namespace ShareHour.Web.Models;
public class ToolItem
{
    public ToolItem() { }
    public ToolItem(string name, decimal hourlyPrice, ToolCategory category)
    { Name = name; HourlyPrice = hourlyPrice; Category = category; }
    public int Id { get; set; }
    [Required, StringLength(80, MinimumLength = 3)]
    public string Name { get; set; } = "";
    [StringLength(400)]
    public string Description { get; set; } = "";
    public ToolCategory Category { get; set; } = ToolCategory.Other;
    [Range(0.1, 10000, ErrorMessage = "Hourly price must be positive.")]
    public decimal HourlyPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public int OwnerId { get; set; }
    public UserAccount Owner { get; set; } = null!;
    public List<Booking> Bookings { get; set; } = new();
    public string RateLabel() => $"OMR {HourlyPrice:0.000}/hour";
}
