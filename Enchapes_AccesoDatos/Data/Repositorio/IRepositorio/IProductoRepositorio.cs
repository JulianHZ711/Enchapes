using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enchapes_Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enchapes_AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto> 
    {
        void Actualizar(Producto producto);

        IEnumerable<SelectListItem> ObtenerTodosDropdownList(string obj);
    }
}
