using System.ComponentModel.DataAnnotations;
namespace ShareHour.Web.Models;
public class UserAccount
{
    public UserAccount() { }
    public UserAccount(string fullName, string email, UserRole role)
    { FullName = fullName; Email = email; Role = role; }
    public int Id { get; set; }
    [Required, StringLength(70, MinimumLength = 2)]
    public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.Renter;
    public List<ToolItem> OwnedTools { get; set; } = new();
    public List<Booking> Bookings { get; set; } = new();
    public string DisplayName() => $"{FullName} ({Role})";
}
