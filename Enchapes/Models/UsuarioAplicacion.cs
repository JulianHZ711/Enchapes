using Microsoft.AspNetCore.Identity;

namespace Enchapes.Models
{
    public class UsuarioAplicacion :IdentityUser
    {
        public string NombreCompleto { get; set; }


    }
}
