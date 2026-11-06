using Microsoft.EntityFrameworkCore;
using ShareHour.Web.Models;
namespace ShareHour.Web.Data;
public class ShareHourDb(DbContextOptions<ShareHourDb> options) : DbContext(options)
{
    public DbSet<UserAccount> Users => Set<UserAccount>();
    public DbSet<ToolItem> Tools => Set<ToolItem>();
    public DbSet<Booking> Bookings => Set<Booking>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserAccount>().Property(u => u.Role).HasConversion<string>();
        modelBuilder.Entity<ToolItem>().Property(t => t.Category).HasConversion<string>();
        modelBuilder.Entity<ToolItem>().Property(t => t.HourlyPrice).HasPrecision(10, 3);
        modelBuilder.Entity<Booking>().Property(b => b.Status).HasConversion<string>();
        modelBuilder.Entity<Booking>().Property(b => b.TotalCost).HasPrecision(10, 3);
        modelBuilder.Entity<ToolItem>().HasOne(t => t.Owner).WithMany(u => u.OwnedTools)
            .HasForeignKey(t => t.OwnerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>().HasOne(b => b.Tool).WithMany(t => t.Bookings)
            .HasForeignKey(b => b.ToolId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>().HasOne(b => b.Renter).WithMany(u => u.Bookings)
            .HasForeignKey(b => b.RenterId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>().HasIndex(b => new { b.ToolId, b.StartAt, b.EndAt });
    }
}
