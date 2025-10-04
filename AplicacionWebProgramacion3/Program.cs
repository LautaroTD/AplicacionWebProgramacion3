using AplicacionWebProgramacion3.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AplicacionWebProgramacion3.Models;

var builder = WebApplication.CreateBuilder(args);


//para hacer las migraciones y todo eso BIEN
//1- Crea la base de datos VACIA
//2- Conectado con EF y pone bien el appsettings. ACORDATE QUE USAR VPN NO TE PERMITE USAR EL EF.
//3- Hace la primera migracion.
//4- Si queres crear nuevas tablas, tendras que crear el modelo en VISUAL STUDIO y luego hacer la migracion (es mas lento que crearlas en SQL)

//otra forma
//1- Crea la base de datos como quieras
//2- Conectado con EF y pone bien el appsettings. ACORDATE QUE USAR VPN NO TE PERMITE USAR EL EF.
//3- Hace la primera migracion
//4- Si queres hacer un cambio o crear nuevas tablas, tendras que crear todas TUS tablas, las tablas que VOS creaste, y luego hace la migracion, se te crearan de vuelta las tablas que borraste

//estoy seguro que hay una mejor forma pero nose lmao.

////Hare cambios poco a poco hasta adaptarlo para usar las tablas de AspNet Identity. Vere si puedo.

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("admin");
    });
    options.AddPolicy("AdminPlanta", policy =>
    {
        policy.RequireRole("adminPlanta");
    });
    options.AddPolicy("AdminFertilizante", policy =>
    {
        policy.RequireRole("adminFertilizante");
    });
    options.AddPolicy("AdminSuelo", policy =>
    {
        policy.RequireRole("adminSuelo");
    });
});

builder.Services.AddAuthentication(options => { options.DefaultScheme = "MyCookieAuth"; })
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;  // O None si es necesario, según contexto
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Para localhost / HTTP
        options.LoginPath = "/Shared/Login";
        options.AccessDeniedPath = "/Shared/Error";
    });

builder.Services.AddDbContext<plantasDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("plantasDBContext")));
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
