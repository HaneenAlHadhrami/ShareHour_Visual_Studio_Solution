using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Data;
using ShareHour.Web.Services;
namespace ShareHour.Web.Pages.Account;
public class LoginModel(ShareHourDb db) : PageModel
{
    [BindProperty] public LoginInput Input { get; set; } = new();
    public class LoginInput
    {
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string Password { get; set; } = "";
    }
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == Input.Email.Trim().ToLower());
        if (user == null || !PasswordUtil.Verify(Input.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Incorrect email or password.");
            return Page();
        }
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName), new Claim(ClaimTypes.Role, user.Role.ToString()) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return RedirectToPage("/Index");
    }
}
