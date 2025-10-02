using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AplicacionWebProgramacion3.Models;

namespace AplicacionWebProgramacion3.Controllers
{
    public class SuelosController : Controller
    {
        private readonly plantasDBContext _context;

        public SuelosController(plantasDBContext context)
        {
            _context = context;
        }

        // GET: Suelos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suelos.ToListAsync());
        }

        // GET: Suelos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suelos = await _context.Suelos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suelos == null)
            {
                return NotFound();
            }

            return View(suelos);
        }

        // GET: Suelos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suelos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Nombre,pH,Tipo")] Suelos suelos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suelos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suelos);
        }

        // GET: Suelos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suelos = await _context.Suelos.FindAsync(id);
            if (suelos == null)
            {
                return NotFound();
            }
            return View(suelos);
        }

        // POST: Suelos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Nombre,pH,Tipo")] Suelos suelos)
        {
            if (id != suelos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suelos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuelosExists(suelos.Id))
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
            return View(suelos);
        }

        // GET: Suelos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suelos = await _context.Suelos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suelos == null)
            {
                return NotFound();
            }

            return View(suelos);
        }

        // POST: Suelos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suelos = await _context.Suelos.FindAsync(id);
            if (suelos != null)
            {
                _context.Suelos.Remove(suelos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuelosExists(int id)
        {
            return _context.Suelos.Any(e => e.Id == id);
        }
    }
}
