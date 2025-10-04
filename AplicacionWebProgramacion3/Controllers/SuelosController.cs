using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AplicacionWebProgramacion3.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Policy = "AdminSuelo")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suelos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdminSuelo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Suelos suelos)
        {
            int newId;
            var random = new Random();
            do
            {
                newId = random.Next(1, int.MaxValue);
            } while (_context.Usuarios.Any(u => u.Id == newId));

            /*
              var newUser = new Usuario
            {
                Id = newId,
                Name = register.Name,
                Role = register.Role,
                Imagen = register.Imagen
            };
            */

            var newSuelo = new Suelos
            {
                Id = newId,
                Nombre = suelos.Nombre,
                PH = suelos.PH,
                Tipo = suelos.Tipo
            };

            /*
            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();
            */

            _context.Suelos.Add(newSuelo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Suelos");
        }

        // GET: Suelos/Edit/5
        [Authorize(Policy = "AdminSuelo")]
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
        [Authorize(Policy ="AdminSuelo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Suelos suelos)
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
        [Authorize(Policy = "AdminSuelo")]
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
        [Authorize(Policy = "AdminSuelo")]
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
