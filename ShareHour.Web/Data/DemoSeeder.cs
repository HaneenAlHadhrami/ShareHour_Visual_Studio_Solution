using ShareHour.Web.Models;
using ShareHour.Web.Services;
namespace ShareHour.Web.Data;
public static class DemoSeeder
{
    public static void Seed(ShareHourDb db)
    {
        if (db.Users.Any()) return;
        // Demonstration credentials only. Replace before any public deployment.
        var owner = new UserAccount { FullName = "Demo Owner", Email = "owner@sharehour.test",
            PasswordHash = PasswordUtil.Hash("Demo123!"), Role = UserRole.Owner };
        var renter = new UserAccount { FullName = "Demo Renter", Email = "renter@sharehour.test",
            PasswordHash = PasswordUtil.Hash("Demo123!"), Role = UserRole.Renter };
        db.Users.AddRange(owner, renter);
        db.SaveChanges();
        db.Tools.AddRange(
            new ToolItem { OwnerId = owner.Id, Name = "Electric Drill", Category = ToolCategory.Home,
                Description = "Cordless power drill for short household jobs.", HourlyPrice = 1.500m },
            new ToolItem { OwnerId = owner.Id, Name = "Garden Trimmer", Category = ToolCategory.Gardening,
                Description = "Lightweight electric trimmer.", HourlyPrice = 2.000m },
            new ToolItem { OwnerId = owner.Id, Name = "Portable Projector", Category = ToolCategory.Electronics,
                Description = "Portable projector for events and presentations.", HourlyPrice = 3.000m }
        );
        db.SaveChanges();
    }
}
