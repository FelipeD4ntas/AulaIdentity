using IdentityProjeto.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.ComponentModel.DataAnnotations;

namespace IdentityProjeto.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminRolesController : Controller
{
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<IdentityUser> _userManager;

	public AdminRolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	[HttpGet]
	public ViewResult Index()
	{
		return View(_roleManager.Roles);
	}

	[HttpPost]
	public async Task<IActionResult> Create([Required] string name)
	{
		if (ModelState.IsValid)
		{
			IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));

			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				Errors(result);
			}
		}

		return View(name);
	}

	[HttpGet]
	public async Task<IActionResult> Update(string id)
	{
		// Obtém uma instância de IdentityRole com o Id especificado
		IdentityRole role = await _roleManager.FindByIdAsync(id);

		// Cria listas vazias de usuários membros e não membros
		List<IdentityUser> members = new List<IdentityUser>();
		List<IdentityUser> nonMembers = new List<IdentityUser>();

		// Itera por todos os usuários na base de dados
		foreach (IdentityUser user in _userManager.Users)
		{
			// Verifica se o usuário pertence ao papel (role) especificado
			// Se pertencer, adiciona o usuário à lista de membros, senão adiciona à lista de não membros
			var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;

			list.Add(user);
		}

		// Retorna uma view chamada "Update" passando um objeto RoleEdit como parâmetro
		// O objeto RoleEdit contém a instância de IdentityRole, as listas de membros e não membros
		return View(new RoleEdit
		{
			Role = role,
			Members = members,
			NonMembers = nonMembers
		});
	}

	[HttpPost]
	public async Task<IActionResult> Update(RoleModification model)
	{
		IdentityResult result;

		// Verifica se o ModelState é válido
		if (ModelState.IsValid)
		{
			// Itera por cada Id de usuário para adicionar ao papel (role)
			foreach (string userId in model.AddIds ?? new string[] { })
			{
				// Obtém uma instância de IdentityUser com o Id especificado
				IdentityUser user = await _userManager.FindByIdAsync(userId);
				if (user != null)
				{
					// Adiciona o usuário ao papel (role) especificado
					result = await _userManager.AddToRoleAsync(user, model.RoleName);

					// Verifica se a operação foi bem-sucedida, caso contrário, chama o método Errors para adicionar os erros ao ModelState
					if (!result.Succeeded)
					{
						Errors(result);
					}
				}
			}

			// Itera por cada Id de usuário para remover do papel (role)
			foreach (string userId in model.DeleteIds ?? new string[] { })
			{
				// Obtém uma instância de IdentityUser com o Id especificado
				IdentityUser user = await _userManager.FindByIdAsync(userId);

				if (user != null)
				{
					// Remove o usuário do papel (role) especificado
					result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

					// Verifica se a operação foi bem-sucedida, caso contrário, chama o método Errors para adicionar os erros ao ModelState
					if (!result.Succeeded)
					{
						Errors(result);
					}
				}
			}
		}

		// Verifica se o ModelState é válido
		if (ModelState.IsValid)
		{
			// Redireciona para a action "Index" se o ModelState for válido
			return RedirectToAction("Index");
		}
		else
		{
			// Chama a action "Update" com o Id do papel (role) se o ModelState for inválido
			return await Update(model.RoleId);
		}
	}

	[HttpGet]
	public async Task<IActionResult> Delete(string Id)
	{
		IdentityRole role = await _roleManager.FindByIdAsync(Id);

		if (role == null)
		{
			ModelState.AddModelError("", "Role não encontrada");
			return View("Index", _roleManager.Roles);
		}

		return View(role);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(string Id)
	{
		IdentityRole role = await _roleManager.FindByIdAsync(Id);

		if (role != null)
		{
			IdentityResult result = await _roleManager.DeleteAsync(role);

			if (result.Succeeded)
			{
				return View("Index");	
			}
			else
			{
				Errors(result);
			}
		}
		else
		{
			ModelState.AddModelError("", "Role não encontrada");
		}

		return View("Index", _roleManager.Roles);
	}


	private void Errors(IdentityResult result)
	{
		foreach (IdentityError error in result.Errors)
		{
			ModelState.AddModelError("", error.Description);
		}
	}
}
