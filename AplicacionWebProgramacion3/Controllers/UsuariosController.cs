using AplicacionWebProgramacion3.DTOs;
using AplicacionWebProgramacion3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWebProgramacion3.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly plantasDBContext _context;

        public UsuariosController(plantasDBContext context)
        {
            _context = context;
        }

        // GET: Usuarios //Nota: aca es la POLITICA, no el ROLE. La politica se define en Program.cs
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: UserController/Create
        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(RegisterDTO register)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Datos inválidos";
                return View();
            }

            var existingUser = _context.Usuarios.FirstOrDefault(u => u.Name == register.Name);
            if (existingUser != null)
            {
                ViewBag.Error = "El usuario ya existe";
                return View();
            }

            var passwordHasher = new PasswordHasher<Usuario>();
            var newUser = new Usuario
            {
                Name = register.Name,
                Role = register.Role,
                Imagen = register.Imagen
            };

            newUser.Password = passwordHasher.HashPassword(newUser, register.Password);

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            // Opcional: loguear automáticamente tras registro

            return RedirectToAction("Index", "User");
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            //tip: si quere pasar una variable del edit get (este) al edit post (el otro), tenes que agregarla en el formulario, las variables de aca se pasan al formulario, las que no se declaran en el formulario se pierden.
            //la linea 'var cable = await _context.CableTable.FindAsync(id);' no te estuvo funcionando porque id es ENTERO y IdCab es STRING. creo. y como las rutas son en int olvidate, cagate. pto. tene que transformarlas si o si.

            var cable = await _context.Usuarios.FindAsync(id);
            if (cable == null)
            {
                return NotFound();
            }
            return View(cable);
        }

        // POST: CableController/Edit/5
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario cable)
        { //Nota CLAVE: la parte de "int id" viene de la RUTA del /CableController/Edit/numero, NO del formulario. de eso viene el objeto. Y por eso te esta cagando, porque el Id de tus tablas son string, y vos usas int porque los generas aleatoriamente. Que gana no?

            if (id != cable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cable);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.Id == cable.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(cable);
        }

        // GET: UsuarioController/Details/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: CableController/Delete/5
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var producto = await _context.Usuarios.FindAsync(id);
            if (producto is null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
