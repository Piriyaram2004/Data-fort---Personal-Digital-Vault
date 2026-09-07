using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeRolesAsync(
            ApplicationDbContext context)
        {
            var userRoleExists = await context.Roles
                .AnyAsync(r => r.RoleName == "User"); // Check if the "User" role already
                                                      // exists on the roles table in the database

            if (!userRoleExists)
            {
                context.Roles.Add(new Role
                {
                    RoleName = "User",
                    Description = "Standard application user",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            var administratorRoleExists = await context.Roles
                .AnyAsync(r => r.RoleName == "Administrator");

            if (!administratorRoleExists)
            {
                context.Roles.Add(new Role
                {
                    RoleName = "Administrator",
                    Description = "System administrator",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync(); //send the changes(pending) to the database
        }
    }
}