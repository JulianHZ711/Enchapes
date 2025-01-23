using Enchapes_AccesoDatos.Data.Repositorio.IRepositorio;
using Enchapes_AccesoDatos.Datos;
using Enchapes_Modelos;
using Enchapes_Modelos.ViewModels;
using Enchapes_Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Enchapes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoRepositorio _productoRepo;
        private readonly ICategoriaRepositorio _categoriaRepo;




        public HomeController(ILogger<HomeController> logger, IProductoRepositorio productoRepositorio, ICategoriaRepositorio categoriaRepo)
        {
            _logger = logger;
            _productoRepo = productoRepositorio;
            _categoriaRepo = categoriaRepo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {

                //Productos = _db.Producto.Include(c=>c.Categoria).Include(t=>t.TipoAplicacion),
                //Categorias = _db.Categoria

                Productos = _productoRepo.ObtenerTodos(incluirPropiedades: "Categoria,TipoAplicacion"),
                Categorias = _categoriaRepo.ObtenerTodos()
            };

            return View(homeVM);
        }

        public IActionResult Detalle(int Id) {

            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            DetalleVM detalleVM = new DetalleVM()
            {
                //Producto = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion)
                //                       .Where(p => p.Id == Id).FirstOrDefault(),

                Producto = _productoRepo.ObtenerPrimero(p => p.Id == Id, incluirPropiedades:"Categoria,TipoAplicacion"),
                ExisteEnCarro = false
            };
            foreach (var item in carroComprasLista)
            {
                if (item.ProductoId == Id)
                {
                    detalleVM.ExisteEnCarro = true;
                }
            }

            return View(detalleVM);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id, DetalleVM detalleVM)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras)!=null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count()>0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            
            carroComprasLista.Add(new CarroCompra {ProductoId = Id, MetroCuadrado = detalleVM.Producto.TempMetroCuadrado});
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }

            var productoARemover = carroComprasLista.SingleOrDefault(x=>x.ProductoId ==Id);
            if (productoARemover != null)
            {
                carroComprasLista.Remove(productoARemover);
            }

            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
