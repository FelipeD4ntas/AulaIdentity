using Microsoft.AspNetCore.Identity;

namespace IdentityProjeto.Services;

public class SeedUserRoleInitial : ISeedUserRoleInitial
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task SeedRolesAsync()
	{
		// Verifica se já existe uma Role chamada User, se não exixtir vai criar.
		if (! await _roleManager.RoleExistsAsync("User"))
		{
			IdentityRole role = new IdentityRole();

			role.Name = "User";
			role.NormalizedName = "USER";
			role.ConcurrencyStamp = Guid.NewGuid().ToString();

			IdentityResult roleResult = await _roleManager.CreateAsync(role);
		}

		// Verifica se já existe uma Role chamada Admin, se não exixtir vai criar.
		if (!await _roleManager.RoleExistsAsync("Admin"))
		{
			IdentityRole role = new IdentityRole();

			role.Name = "Admin";
			role.NormalizedName = "ADMIN";
			role.ConcurrencyStamp = Guid.NewGuid().ToString();

			IdentityResult roleResult = await _roleManager.CreateAsync(role);
		}

		// Verifica se já existe uma Role chamada Gerente, se não exixtir vai criar.
		if (!await _roleManager.RoleExistsAsync("Gerente"))
		{
			IdentityRole role = new IdentityRole();

			role.Name = "Gerente";
			role.NormalizedName = "GERENTE";
			role.ConcurrencyStamp = Guid.NewGuid().ToString();

			IdentityResult roleResult = await _roleManager.CreateAsync(role);
		}
	}

	public async Task SeedUsersAync()
	{
		if (await _userManager.FindByEmailAsync("usuario@localhost") == null) 
		{
			IdentityUser user = new IdentityUser();

			user.UserName = "usuario@localhost";
			user.Email = "usuario@localhost";
			user.NormalizedUserName = "usuario@localhost".ToUpper();
			user.NormalizedEmail = "usuario@localhost".ToUpper();
			user.EmailConfirmed = true;
			user.LockoutEnabled = false;
			user.SecurityStamp = Guid.NewGuid().ToString();

			IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "User");
			}
		}

		if (await _userManager.FindByEmailAsync("admin@localhost") == null)
		{
			IdentityUser user = new IdentityUser();

			user.UserName = "admin@localhost";
			user.Email = "admin@localhost";
			user.NormalizedUserName = "admin@localhost".ToUpper();
			user.NormalizedEmail = "admin@localhost".ToUpper();
			user.EmailConfirmed = true;
			user.LockoutEnabled = false;
			user.SecurityStamp = Guid.NewGuid().ToString();

			IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "Admin");
			}
		}

		if (await _userManager.FindByEmailAsync("gerente@localhost") == null)
		{
			IdentityUser user = new IdentityUser();

			user.UserName = "gerente@localhost";
			user.Email = "gerente@localhost";
			user.NormalizedUserName = "gerente@localhost".ToUpper();
			user.NormalizedEmail = "gerente@localhost".ToUpper();
			user.EmailConfirmed = true;
			user.LockoutEnabled = false;
			user.SecurityStamp = Guid.NewGuid().ToString();

			IdentityResult result = await _userManager.CreateAsync(user, "Numsey#2023");

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "Gerente");
			}
		}
	}
}
