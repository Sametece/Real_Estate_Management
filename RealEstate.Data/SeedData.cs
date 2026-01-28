using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Enum;

namespace RealEstate.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var context = serviceProvider.GetRequiredService<RealEstateDbContext>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Roles
        await SeedRoles(roleManager);

        // Seed Users
        await SeedUsers(userManager);

        // Seed Property Types
        await SeedPropertyTypes(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        var roles = new[] { "Admin", "Agent", "User" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole
                {
                    Name = roleName,
                    Description = $"{roleName} role for Real Estate Management System"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        // Admin User
        if (await userManager.FindByEmailAsync("admin@test.com") == null)
        {
            var adminUser = new AppUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                FirstName = "Admin",
                LastName = "User",
                IsAgent = false,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Agent User
        if (await userManager.FindByEmailAsync("agent@test.com") == null)
        {
            var agentUser = new AppUser
            {
                UserName = "agent@test.com",
                Email = "agent@test.com",
                FirstName = "Agent",
                LastName = "Smith",
                IsAgent = true,
                AgencyName = "Smith Real Estate",
                LicenseNumber = "RE123456",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(agentUser, "Agent123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(agentUser, "Agent");
            }
        }

        // Regular User
        if (await userManager.FindByEmailAsync("user@test.com") == null)
        {
            var regularUser = new AppUser
            {
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "John",
                LastName = "Doe",
                IsAgent = false,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(regularUser, "User123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }

    private static async Task SeedPropertyTypes(RealEstateDbContext context)
    {
        if (!context.PropertyTypes.Any())
        {
            var propertyTypes = new[]
            {
                new PropertyType { Name = "Daire", Description = "Apartman dairesi" },
                new PropertyType { Name = "Villa", Description = "Müstakil villa" },
                new PropertyType { Name = "Müstakil Ev", Description = "Müstakil ev" },
                new PropertyType { Name = "Dükkan", Description = "Ticari dükkan" },
                new PropertyType { Name = "Ofis", Description = "Ofis alanı" },
                new PropertyType { Name = "Arsa", Description = "İnşaat arsası" },
                new PropertyType { Name = "İşyeri", Description = "Ticari işyeri" }
            };

            context.PropertyTypes.AddRange(propertyTypes);
        }
    }
}