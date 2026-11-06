using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Models;
using ShareHour.Web.Services;
namespace ShareHour.Web.Pages.Account;
public class RegisterModel(ShareHourDb db) : PageModel
{
    [BindProperty] public RegistrationInput Input { get; set; } = new();
    public class RegistrationInput
    {
        [Required, StringLength(70, MinimumLength = 2)] public string FullName { get; set; } = "";
        [Required, EmailAddress, StringLength(120)] public string Email { get; set; } = "";
        [Required, StringLength(50, MinimumLength = 8)] public string Password { get; set; } = "";
        [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = "";
        [Required] public UserRole Role { get; set; } = UserRole.Renter;
    }
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        string email = Input.Email.Trim().ToLower();
        if (await db.Users.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError("Input.Email", "This email is already registered.");
            return Page();
        }
        if (!Enum.IsDefined(Input.Role))
        {
            ModelState.AddModelError("Input.Role", "Invalid role.");
            return Page();
        }
        try
        {
            db.Users.Add(new UserAccount { FullName = Input.FullName.Trim(), Email = email,
                PasswordHash = PasswordUtil.Hash(Input.Password), Role = Input.Role });
            await db.SaveChangesAsync();
            TempData["Notice"] = "Account created. Please sign in.";
            return RedirectToPage("/Account/Login");
        }
        catch (Exception) { ModelState.AddModelError("", "Could not create account. Please try again."); return Page(); }
    }
}
