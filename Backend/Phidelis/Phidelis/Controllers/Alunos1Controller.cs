using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Context;
using Application.Models;

namespace Phidelis.WebAPI.Controllers
{
    public class Alunos1Controller : Controller
    {
        private readonly PhidelisContext _context;

        public Alunos1Controller(PhidelisContext context)
        {
            _context = context;
        }

        // GET: Alunos1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Alunos.ToListAsync());
        }

        // GET: Alunos1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunos = await _context.Alunos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alunos == null)
            {
                return NotFound();
            }

            return View(alunos);
        }

        // GET: Alunos1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alunos1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cpf,Nascimento,Matricula")] Alunos alunos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alunos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alunos);
        }

        // GET: Alunos1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunos = await _context.Alunos.FindAsync(id);
            if (alunos == null)
            {
                return NotFound();
            }
            return View(alunos);
        }

        // POST: Alunos1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cpf,Nascimento,Matricula")] Alunos alunos)
        {
            if (id != alunos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alunos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunosExists(alunos.Id))
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
            return View(alunos);
        }

        // GET: Alunos1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alunos = await _context.Alunos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alunos == null)
            {
                return NotFound();
            }

            return View(alunos);
        }

        // POST: Alunos1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alunos = await _context.Alunos.FindAsync(id);
            _context.Alunos.Remove(alunos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunosExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }
    }
}
