using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Enchapes_AccesoDatos.Data.Repositorio.IRepositorio;
using Enchapes_AccesoDatos.Datos;
using Enchapes_Modelos;
using Enchapes_Utilidades;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Enchapes_AccesoDatos.Data.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {

        private readonly ApplicationDbContext _db;
     

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }
        public void Actualizar(Producto producto)
        {
            _db.Update(producto);
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownList(string obj)
        {
            if (obj == WC.CategoriaNombre)
            {
                return _db.Categoria.Select(i => new SelectListItem
                {
                    Text = i.NombreCategoria,
                    Value = i.Id.ToString()
                });

            }
            if (obj == WC.TipoAplicacionNombre)
            {
                return _db.TipoAplicacion.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                });
            }

            return null;
        }
    }
}
