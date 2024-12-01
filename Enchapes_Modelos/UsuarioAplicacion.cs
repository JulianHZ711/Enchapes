using Microsoft.AspNetCore.Identity;

namespace Enchapes_Modelos
{
    public class UsuarioAplicacion :IdentityUser
    {
        public string NombreCompleto { get; set; }


    }
}
