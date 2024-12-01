using System.ComponentModel.DataAnnotations;

namespace Enchapes_Modelos
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre del tipo de aplicación es obligatorio")]
        public string Nombre { get; set; }

    }
}
