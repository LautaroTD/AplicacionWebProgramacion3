using AplicacionWebProgramacion3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

//ESTE ARCHIVO... no se si es clave o ando porque arregle el appsetting.json. SIN EMBARGO, SI ESTE PROYECTO FUESE MUY COMPLICADO SI HARIA FALTA.

namespace AplicacionWebProgramacion3.Data // Cambia este namespace si es diferente en tu proyecto
{
    public class plantasDBContextFactory : IDesignTimeDbContextFactory<plantasDBContext>
    {
        public plantasDBContext CreateDbContext(string[] args)
        {
            // Construye la configuración leyendo appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtén la cadena de conexión
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<plantasDBContext>();
            optionsBuilder.UseSqlServer(connectionString); // Cambia a UseSqlite o UseMySql si usas otra base de datos

            return new plantasDBContext(optionsBuilder.Options);
        }
    }
}
