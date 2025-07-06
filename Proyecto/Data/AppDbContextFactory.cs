using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Proyecto.Data
{
    public class AppDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1. Carga appsettings.json desde la carpeta raíz
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();

            // 2. Obtén la cadena de conexión "Default"
            var conn = config.GetConnectionString("Default");
            if (string.IsNullOrEmpty(conn))
                throw new InvalidOperationException(
                  "No se encontró la cadena de conexión 'Default' en appsettings.json");

            // 3. Construye las opciones del DbContext
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(conn);

            // 4. Devuelve una instancia configurada
            return new AppDbContext(builder.Options);
        }
    }
}
