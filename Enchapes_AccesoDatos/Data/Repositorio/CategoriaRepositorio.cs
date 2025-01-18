using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Enchapes_AccesoDatos.Data.Repositorio.IRepositorio;
using Enchapes_AccesoDatos.Datos;
using Enchapes_Modelos;
using Microsoft.EntityFrameworkCore;

namespace Enchapes_AccesoDatos.Data.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {

        private readonly ApplicationDbContext _db;
     

        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }
        public void Actualizar(Categoria categoria)
        {
            var catAnterior = _db.Categoria.FirstOrDefault(c => c.Id == categoria.Id);
            if (catAnterior != null)
            {
                catAnterior.NombreCategoria = categoria.NombreCategoria;
                catAnterior.MostrarOrden = categoria.MostrarOrden;
            }
        }

    }
}
