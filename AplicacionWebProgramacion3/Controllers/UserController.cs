using AplicacionWebProgramacion3.DTOs;
using AplicacionWebProgramacion3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AplicacionWebProgramacion3.Controllers
{
    public class UserController : Controller
    {
        private readonly plantasDBContext _context;

        public UserController(plantasDBContext context)
        {
            _context = context;
        }
        // GET: UserController
        public ActionResult Index()
        {
            var resultados = _context.Usuarios.ToList();
            return View(resultados);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string contrasena, string rol)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Datos inválidos";
                return View();
            }

            var existingUser = _context.Usuarios.FirstOrDefault(u => u.Name == nombre && u.Role == rol);
            if (existingUser != null)
            {
                ViewBag.Error = "El usuario ya existe";
                return View();
            }

            Random rnd = new Random();


            int IdNueva;

            do
            {
                IdNueva = rnd.Next(-999999999, 999999999);

                existingUser = _context.Usuarios.FirstOrDefault(u => u.Id == IdNueva); //en esta el nombre que sale en el plantasDbContext
            }
            while (existingUser != null);


            var passwordHasher = new PasswordHasher<Usuario>(); //en esta linea el modelo
            var newUser = new Usuario //aca tambien el modelo
            {
                Id = IdNueva,
                Name = nombre,
                Role = rol
            };

            newUser.Password = passwordHasher.HashPassword(newUser, contrasena);

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            // Opcional: loguear automáticamente tras registro

            return RedirectToAction("Index", "Usuario");
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var producto = await _context.Usuarios.FindAsync(id);
            if(producto is null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        { //login que me manda a la vista
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(RegisterDTO register)
        { //login que se ejecuta despues de mandar el form
            var user = _context.Usuarios.FirstOrDefault(u => u.Name == register.Name);

            if (user != null)
            {
                await HttpContext.SignOutAsync("MyCookieAuth");

                // Opcional: limpiar más cookies
                Response.Cookies.Delete("MyCookieAuth");

                var passwordHasher = new PasswordHasher<Usuario>();
                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, register.Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("MyCookieAuth", principal);

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Usuario o contraseña inválidos";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");

            // Opcional: limpiar más cookies
            Response.Cookies.Delete("MyCookieAuth");

            return Redirect("/Home/Index"); //esto es mejor que "return RedirectToAction("Index", "Home");".
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register)
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

            await HttpContext.SignOutAsync("MyCookieAuth");

            // Opcional: limpiar más cookies
            Response.Cookies.Delete("MyCookieAuth");

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, newUser.Name),
            new Claim(ClaimTypes.Role, newUser.Role) //Esto es muy importante, pues puedes usar este "Role" junto el "[Authorize]", algo asi "[Authorize(Roles = "Administrador")]"
            };

            newUser.Password = passwordHasher.HashPassword(newUser, register.Password);

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            // Opcional: loguear automáticamente tras registro

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            return RedirectToAction("Index", "Home");
        }
    }
}
