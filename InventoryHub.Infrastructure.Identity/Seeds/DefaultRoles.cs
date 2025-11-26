using InventoryHub.Core.Application.Enums;
using InventoryHub.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventoryHub.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
	{
		public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
		}
	}
}
