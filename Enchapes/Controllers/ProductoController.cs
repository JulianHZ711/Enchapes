using Enchapes_AccesoDatos.Datos;
using Enchapes_Modelos;
using Enchapes_Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Enchapes_Utilidades;
using Enchapes_AccesoDatos.Data.Repositorio.IRepositorio;

namespace Enchapes.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductoController : Controller
    {

        private readonly IProductoRepositorio _productoRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IProductoRepositorio productoRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productoRepo = productoRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //Se utiliza Patron de repositorio
            //IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria)
            //                                           .Include(t=> t.TipoAplicacion);

            IEnumerable<Producto> lista = _productoRepo.ObtenerTodos(incluirPropiedades: "Categoria,TipoAplicacion");
            return View(lista);
        }

        //Get
        public IActionResult Upsert(int? Id) 
        {

            //IEnumerable<SelectListItem> categoriaDropdown = _db.Categoria.Select(c => new SelectListItem
            //{
            //    Text = c.NombreCategoria,
            //    Value = c.Id.ToString()
            //});

            //ViewBag.categoriaDropdown = categoriaDropdown;

            //Producto producto = new Producto();

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                // CategoriaLista =    _db.Categoria.Select(c => new SelectListItem
                // {
                //Text = c.NombreCategoria,
                // Value = c.Id.ToString()
                // }),

                // TipoAplicacionLista = _db.TipoAplicacion.Select(c => new SelectListItem
                // {
                //     Text = c.Nombre,
                //     Value = c.Id.ToString()
                // })

                CategoriaLista = _productoRepo.ObtenerTodosDropdownList(WC.CategoriaNombre),
                TipoAplicacionLista = _productoRepo.ObtenerTodosDropdownList(WC.TipoAplicacionNombre)

            };

            if (Id == null)
            {
                //Crear nuevo producto
                return View(productoVM);
                
            }
            else
            {
                productoVM.Producto = _productoRepo.Obtener(Id.GetValueOrDefault());
                if (productoVM == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid) {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.Producto.Id == 0)
                {
                    //Crear
                    string upload = webRootPath + WC.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var filesStream = new FileStream(Path.Combine(upload, fileName+extension),FileMode.Create)) {
                        files[0].CopyTo(filesStream);
                    }

                    productoVM.Producto.ImagenUrl = fileName+extension;
                    _productoRepo.Agregar(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    //var objProducto = _productoRepo..FirstOrDefault(p=>p.Id == productoVM.Producto.Id);
                    var objProducto = _productoRepo.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);

                    if (files.Count > 0) //Se carga nueva imagen
                    {
                        string upload = webRootPath + WC.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Borrar imagen anterior
                        var anteriorFile=Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        //Fin borrar imagen

                        using (var filesStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(filesStream);
                        }

                        productoVM.Producto.ImagenUrl = fileName+extension;
                    } //Caso contario
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _productoRepo.Actualizar(productoVM.Producto);


                }
                _productoRepo.Grabar();
                return RedirectToAction("Index");
                // If modelIsValid
            }
            //Se llennan nuevamente las vistas por si algo falla
            //productoVM.CategoriaLista = _db.Categoria.Select(c => new SelectListItem
            //{
            //    Text = c.NombreCategoria,
            //    Value = c.Id.ToString()
            //});

            //productoVM.TipoAplicacionLista = _db.TipoAplicacion.Select(c => new SelectListItem
            //{
            //    Text = c.Nombre,
            //    Value = c.Id.ToString()
            //});

            productoVM.CategoriaLista = _productoRepo.ObtenerTodosDropdownList(WC.CategoriaNombre);
            productoVM.TipoAplicacionLista = _productoRepo.ObtenerTodosDropdownList(WC.TipoAplicacionNombre);


            return View(productoVM);

        }

        //Get
        public IActionResult Eliminar(int? Id)
        {
            if (Id==null || Id ==0)
            {
                return NotFound();
            }

            Producto producto = _productoRepo.ObtenerPrimero(p=>p.Id==Id, incluirPropiedades: "Categoria,TipoAplicacion");

            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {
            if (producto == null) {

                return NotFound();
            }
            //Eliminar imagen

            string upload = _webHostEnvironment.WebRootPath + WC.ImagenRuta;

            //Borrar imagen anterior
            var anteriorFile = Path.Combine(upload, producto.ImagenUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            //Fin borrar imagen

            _productoRepo.Remover(producto);
            _productoRepo.Grabar();
            return RedirectToAction(nameof(Index));
        }

    }
}
