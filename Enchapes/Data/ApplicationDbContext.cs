using Enchapes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Enchapes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<TipoAplicacion> TipoAplicacion { get; set; }

        public DbSet<Producto> Producto { get; set; }

        public DbSet<UsuarioAplicacion> usuarioAplicacion { get; set; }
    }
}
