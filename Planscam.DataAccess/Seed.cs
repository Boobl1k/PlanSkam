using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planscam.Entities;

namespace Planscam.DataAccess;

internal static class Seed
{
    public static ModelBuilder CreateRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "Author",
            });
        return modelBuilder;
    }

    public static ModelBuilder CreateSubscriptions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>().HasData(
            new Subscription
            {
                Name = "Month",
                Description = "Month",
                Price = 100,
                Duration = TimeSpan.FromDays(30)
            },
            new Subscription
            {
                Name = "3 months",
                Description = "3 months",
                Price = 250,
                Duration = TimeSpan.FromDays(91)
            },
            new Subscription
            {
                Name = "Year",
                Description = "Year",
                Price = 800,
                Duration = TimeSpan.FromDays(365)
            });
        return modelBuilder;
    }
}
