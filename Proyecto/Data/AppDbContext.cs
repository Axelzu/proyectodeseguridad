using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Activo> Activos => Set<Activo>();
        public DbSet<Riesgo> Riesgos { get; set; }

        public DbSet<Control> Controles => Set<Control>();
    }
}
