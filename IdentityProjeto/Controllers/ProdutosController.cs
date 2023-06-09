﻿using IdentityProjeto.Context;
using IdentityProjeto.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityProjeto.Controllers
{
	public class ProdutosController : Controller
	{
		private readonly AppDbContext _context;

		public ProdutosController(AppDbContext context)
		{
			_context = context;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			return _context.Produtos != null ?
						View(await _context.Produtos.ToListAsync()) :
						Problem("Entity set 'AppDbContext.Produtos'  is null.");
		}

		[Authorize(Policy = "IsAdminAcess")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Produtos == null)
			{
				return NotFound();
			}

			var produto = await _context.Produtos
				.FirstOrDefaultAsync(m => m.Id == id);
			if (produto == null)
			{
				return NotFound();
			}

			return View(produto);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "IsFuncionarioAcess")]
		public async Task<IActionResult> Create([Bind("Id,Nome,Preco")] Produto produto)
		{
			if (ModelState.IsValid)
			{
				_context.Add(produto);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(produto);
		}

		[Authorize(Policy = "IsAdminAcess")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Produtos == null)
			{
				return NotFound();
			}

			var produto = await _context.Produtos.FindAsync(id);
			if (produto == null)
			{
				return NotFound();
			}
			return View(produto);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Preco")] Produto produto)
		{
			if (id != produto.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(produto);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProdutosExists(produto.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(produto);
		}

		[Authorize(Roles = "Gerente")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Produtos == null)
			{
				return NotFound();
			}

			var produto = await _context.Produtos
				.FirstOrDefaultAsync(m => m.Id == id);
			if (produto == null)
			{
				return NotFound();
			}

			return View(produto);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Produtos == null)
			{
				return Problem("Entity set 'AppDbContext.Alunos'  is null.");
			}
			var produto = await _context.Produtos.FindAsync(id);
			if (produto != null)
			{
				_context.Produtos.Remove(produto);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProdutosExists(int id)
		{
			return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
