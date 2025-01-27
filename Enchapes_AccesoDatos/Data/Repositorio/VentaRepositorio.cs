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
    public class VentaRepositorio : Repositorio<Venta>, IVentaRepositorio
    {

        private readonly ApplicationDbContext _db;
     

        public VentaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
            
        }
        public void Actualizar(Venta venta)
        {
            _db.Update(venta);
        }

    }
}
