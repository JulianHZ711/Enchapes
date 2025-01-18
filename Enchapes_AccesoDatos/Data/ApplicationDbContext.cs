using Enchapes_Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Enchapes_AccesoDatos.Datos
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

        public DbSet<Orden> Orden { get; set; }

        public DbSet<OrdenDetalle> OrdenDetalle { get; set; }
    }
}
