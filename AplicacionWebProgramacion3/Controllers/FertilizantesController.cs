using AplicacionWebProgramacion3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWebProgramacion3.Controllers
{
    public class FertilizantesController : Controller
    {
        private readonly plantasDBContext _context;

        public FertilizantesController(plantasDBContext context)
        {
            _context = context;
        }

        // GET: Fertilizantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fertilizantes.ToListAsync());
        }

        // GET: Fertilizantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizantes = await _context.Fertilizantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fertilizantes == null)
            {
                return NotFound();
            }

            return View(fertilizantes);
        }

        // GET: Fertilizantes/Create
        [Authorize(Policy = "AdminFertilizante")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fertilizantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdminFertilizante")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Forma,Composicion,Tipo,Descripcion")] Fertilizantes fertilizantes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fertilizantes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fertilizantes);
        }

        // GET: Fertilizantes/Edit/5
        [Authorize(Policy = "AdminFertilizante")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizantes = await _context.Fertilizantes.FindAsync(id);
            if (fertilizantes == null)
            {
                return NotFound();
            }
            return View(fertilizantes);
        }

        // POST: Fertilizantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdminFertilizante")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Forma,Composicion,Tipo,Descripcion")] Fertilizantes fertilizantes)
        {
            if (id != fertilizantes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fertilizantes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FertilizantesExists(fertilizantes.Id))
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
            return View(fertilizantes);
        }

        // GET: Fertilizantes/Delete/5
        [Authorize(Policy = "AdminFertilizante")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fertilizantes = await _context.Fertilizantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fertilizantes == null)
            {
                return NotFound();
            }

            return View(fertilizantes);
        }

        // POST: Fertilizantes/Delete/5
        [Authorize(Policy = "AdminFertilizante")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fertilizantes = await _context.Fertilizantes.FindAsync(id);
            if (fertilizantes != null)
            {
                _context.Fertilizantes.Remove(fertilizantes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FertilizantesExists(int id)
        {
            return _context.Fertilizantes.Any(e => e.Id == id);
        }
    }
}
