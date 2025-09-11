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
    public class PlantasController : Controller
    {
        private readonly plantasDBContext _context;

        public PlantasController(plantasDBContext context)
        {
            _context = context;
        }

        // GET: Plantas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plantas.ToListAsync());
        }

        // GET: Plantas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantas = await _context.Plantas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plantas == null)
            {
                return NotFound();
            }

            return View(plantas);
        }

        // GET: Plantas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plantas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreCientifico,NombreVulgar,Autor,EpocaFloracion,AlturaMaxima,Descripcion")] Plantas plantas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantas);
        }

        // GET: Plantas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantas = await _context.Plantas.FindAsync(id);
            if (plantas == null)
            {
                return NotFound();
            }
            return View(plantas);
        }

        // POST: Plantas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCientifico,NombreVulgar,Autor,EpocaFloracion,AlturaMaxima,Descripcion")] Plantas plantas)
        {
            if (id != plantas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantasExists(plantas.Id))
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
            return View(plantas);
        }

        // GET: Plantas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantas = await _context.Plantas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plantas == null)
            {
                return NotFound();
            }

            return View(plantas);
        }

        // POST: Plantas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantas = await _context.Plantas.FindAsync(id);
            if (plantas != null)
            {
                _context.Plantas.Remove(plantas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantasExists(int id)
        {
            return _context.Plantas.Any(e => e.Id == id);
        }
    }
}
