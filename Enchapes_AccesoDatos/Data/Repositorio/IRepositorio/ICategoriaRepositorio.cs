using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enchapes_Modelos;

namespace Enchapes_AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria> 
    {
        void Actualizar(Categoria categoria);

    }
}
