using AplicacionWebProgramacion3.DTOs;
using AplicacionWebProgramacion3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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

        [AllowAnonymous]
        [HttpGet]
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

            // Generar un ID aleatorio único para el nuevo usuario
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
                Role = "basico",
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
