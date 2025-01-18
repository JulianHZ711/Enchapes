using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enchapes_Modelos;

namespace Enchapes_AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface ITipoAplicacionRepositorio : IRepositorio<TipoAplicacion> 
    {
        void Actualizar(TipoAplicacion tipoAplicacion);

    }
}
