﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityProjeto.Context;
using IdentityProjeto.Entities;
using Microsoft.AspNetCore.Authorization;

namespace IdentityProjeto.Controllers;

[Authorize]
public class AlunosController : Controller
{
    private readonly AppDbContext _context;

    public AlunosController(AppDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
          return _context.Alunos != null ? 
                      View(await _context.Alunos.ToListAsync()) :
                      Problem("Entity set 'AppDbContext.Alunos'  is null.");
    }

    [Authorize(Roles = "User, Admin, Gerente")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Alunos == null)
        {
            return NotFound();
        }

        var aluno = await _context.Alunos
            .FirstOrDefaultAsync(m => m.AlunoId == id);
        if (aluno == null)
        {
            return NotFound();
        }

        return View(aluno);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    //[Authorize(Roles = "User, Admin, Gerente")]
    [Authorize(Policy = "RequireUserAdminGerenteRole")]
		public async Task<IActionResult> Create([Bind("AlunoId,Nome,Email,Idade,Curso")] Aluno aluno)
    {
        if (ModelState.IsValid)
        {
            _context.Add(aluno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(aluno);
    }

    [Authorize(Roles = "Admin, Gerente")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Alunos == null)
        {
            return NotFound();
        }

        var aluno = await _context.Alunos.FindAsync(id);
        if (aluno == null)
        {
            return NotFound();
        }
        return View(aluno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AlunoId,Nome,Email,Idade,Curso")] Aluno aluno)
    {
        if (id != aluno.AlunoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(aluno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlunoExists(aluno.AlunoId))
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
        return View(aluno);
    }

		[Authorize(Roles = "Gerente")]
		public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Alunos == null)
        {
            return NotFound();
        }

        var aluno = await _context.Alunos
            .FirstOrDefaultAsync(m => m.AlunoId == id);
        if (aluno == null)
        {
            return NotFound();
        }

        return View(aluno);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Alunos == null)
        {
            return Problem("Entity set 'AppDbContext.Alunos'  is null.");
        }
        var aluno = await _context.Alunos.FindAsync(id);
        if (aluno != null)
        {
            _context.Alunos.Remove(aluno);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/Account/AccessDenied")]
    public ActionResult AccessDenied()
    {
        return View();
    }

    private bool AlunoExists(int id)
    {
      return (_context.Alunos?.Any(e => e.AlunoId == id)).GetValueOrDefault();
    }
}
