using AplicacionWebProgramacion3.DTOs;
using AplicacionWebProgramacion3.Models;
using AplicacionWebProgramacion3.ViewModel;
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
            /*
              var model = new UserViewModel
             {
                 Roles = new List<SelectListItem>
                 {
                     new SelectListItem { Value = "Admin", Text = "Administrador" },
                     new SelectListItem { Value = "User", Text = "Usuario" },
                     new SelectListItem { Value = "Guest", Text = "Invitado" }
                 }
             };

            var model = new UserViewModel
            {
                Roles = //esto en teoria anda igual que el de arriba, pero esta simplificado
        [
            new() { Value = "Admin", Text = "admin" },
            new() { Value = "Admin de Planta", Text = "adminPlanta" },
            new() { Value = "Admin de Suelo", Text = "adminSuelo" },
            new() { Value = "Admin de Fertilizante", Text = "adminFertilizante" }
        ]
            };
            */

            ViewBag.Roles = new List<SelectListItem > //forma que recomiendo copilot, quizas mejor que la de chatgpt? (son lo mismo me shupa un huebo)
    {// acordate que el primero es Value, que es el valor que va a la base de datos. El SEGUNDO es Text, que es el texto que aparece.
        new SelectListItem { Value = "admin", Text = "Admin" },
        new SelectListItem { Value = "adminPlanta", Text = "Admin de Planta" },
        new SelectListItem { Value = "adminSuelo", Text = "Admin de Suelo" },
        new SelectListItem { Value = "adminFertilizante", Text = "Admin de Fertilizante" },
        new SelectListItem { Value = "basico", Text = "Basico" }
        // Agrega más roles según sea necesario
    };
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

            int newId;
            var random = new Random();
            do
            {
                newId = random.Next(1, int.MaxValue);
            } while (_context.Usuarios.Any(u => u.Id == newId));

            var passwordHasher = new PasswordHasher<Usuario>();
            var newUser = new Usuario
            {
                Id = newId,
                Name = register.Name,
                Role = register.Role,
                Imagen = register.Imagen
            };

            newUser.Password = passwordHasher.HashPassword(newUser, register.Password);

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            // Opcional: loguear automáticamente tras registro

            return RedirectToAction("Index", "Usuarios");
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            //tip: si quere pasar una variable del edit get (este) al edit post (el otro), tenes que agregarla en el formulario, las variables de aca se pasan al formulario, las que no se declaran en el formulario se pierden.
            //la linea 'var cable = await _context.CableTable.FindAsync(id);' no te estuvo funcionando porque id es ENTERO y IdCab es STRING. creo. y como las rutas son en int olvidate, cagate. pto. tene que transformarlas si o si.

            ViewBag.Roles = new List<SelectListItem> //forma que recomiendo copilot, quizas mejor que la de chatgpt? (son lo mismo me shupa un huebo)
    {// acordate que el primero es Value, que es el valor que va a la base de datos. El SEGUNDO es Text, que es el texto que aparece.
        new SelectListItem { Value = "admin", Text = "Admin" },
        new SelectListItem { Value = "adminPlanta", Text = "Admin de Planta" },
        new SelectListItem { Value = "adminSuelo", Text = "Admin de Suelo" },
        new SelectListItem { Value = "adminFertilizante", Text = "Admin de Fertilizante" },
        new SelectListItem { Value = "basico", Text = "Basico" }
        // Agrega más roles según sea necesario
    };

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
        public async Task<IActionResult> Edit(int id, Usuario user)
        { //Nota CLAVE: la parte de "int id" viene de la RUTA del /CableController/Edit/numero, NO del formulario. de eso viene el objeto. Y por eso te esta cagando, porque el Id de tus tablas son string, y vos usas int porque los generas aleatoriamente. Que gana no?

            ViewBag.Roles = new List<SelectListItem> //forma que recomiendo copilot, quizas mejor que la de chatgpt? (son lo mismo me shupa un huebo)
    {// acordate que el primero es Value, que es el valor que va a la base de datos. El SEGUNDO es Text, que es el texto que aparece.
        new SelectListItem { Value = "admin", Text = "Admin" },
        new SelectListItem { Value = "adminPlanta", Text = "Admin de Planta" },
        new SelectListItem { Value = "adminSuelo", Text = "Admin de Suelo" },
        new SelectListItem { Value = "adminFertilizante", Text = "Admin de Fertilizante" },
        new SelectListItem { Value = "basico", Text = "Basico" }
        // Agrega más roles según sea necesario
    };

            var usuarioOriginal = await _context.Usuarios.FindAsync(id);
            if (usuarioOriginal == null)
                return NotFound();

            // Actualizar solo los campos permitidos
            usuarioOriginal.Name = user.Name;
            usuarioOriginal.Role = user.Role;
            usuarioOriginal.Imagen = user.Imagen;
            // Si quieres permitir cambiar la contraseña, aquí deberías hashearla

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Usuarios");
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
