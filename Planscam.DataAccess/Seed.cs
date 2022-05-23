using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Planscam.DataAccess;

internal static class Seed
{
    public static ModelBuilder CreateEntities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "Author",
            });
        return modelBuilder;
    }
}
