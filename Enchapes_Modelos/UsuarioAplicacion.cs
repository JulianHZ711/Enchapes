using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Enchapes_Modelos
{
    public class UsuarioAplicacion :IdentityUser
    {
        public string NombreCompleto { get; set; }

        [NotMapped]
        public string Direccion { get; set; }

        [NotMapped]
        public string Ciudad { get; set; }

    }
}
