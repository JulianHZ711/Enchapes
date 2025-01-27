﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enchapes_Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enchapes_AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface IVentaRepositorio : IRepositorio<Venta> 
    {
        void Actualizar(Venta venta);

        
    }
}
