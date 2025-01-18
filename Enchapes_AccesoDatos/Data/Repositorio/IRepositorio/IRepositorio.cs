using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Enchapes_AccesoDatos.Data.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {

        T Obtener(int id);

        IEnumerable<T> ObtenerTodos(
            //Filtro de búsqueda
            Expression<Func<T, bool>> filtro = null,
            //Función de ordenamiento
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenarBy = null,
            //Propiedades a incluir
            string incluirPropiedades = null,
            //Si se desea el seguimiento de los objetos
            bool isTracking = true
            );

        T ObtenerPrimero(
            //Filtro de búsqueda
            Expression<Func<T, bool>> filtro = null,
            //Propiedades a incluir
            string incluirPropiedades = null,
            //Si se desea el seguimiento de los objetos
            bool isTracking = true
            );

        void Agregar(T entidad);

        void Remover(T entidad);

        void RemoverRango(IEnumerable<T> entidad);

        void Grabar();

    }
}
