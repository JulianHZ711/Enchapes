using Enchapes.Data;
using Enchapes.Models;
using Enchapes.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Enchapes_Utilidades;

namespace Enchapes.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductoController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(ApplicationDbContext db , IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria)
                                                       .Include(t=> t.TipoAplicacion);

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
                CategoriaLista =    _db.Categoria.Select(c => new SelectListItem
                {
               Text = c.NombreCategoria,
                Value = c.Id.ToString()
                }),

                TipoAplicacionLista = _db.TipoAplicacion.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }),

            };

            if (Id == null)
            {
                //Crear nuevo producto
                return View(productoVM);
                
            }
            else
            {
                productoVM.Producto = _db.Producto.Find(Id);
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
                    _db.Producto.Add(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    var objProducto = _db.Producto.AsNoTracking().FirstOrDefault(p=>p.Id == productoVM.Producto.Id);

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
                    _db.Producto.Update(productoVM.Producto);


                }
                _db.SaveChanges();
                return RedirectToAction("Index");
                // If modelIsValid
            }
            //Se llennan nuevamente las vistas por si algo falla
            productoVM.CategoriaLista = _db.Categoria.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });

            productoVM.TipoAplicacionLista = _db.TipoAplicacion.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()
            });

            return View(productoVM);

        }

        //Get
        public IActionResult Eliminar(int? Id)
        {
            if (Id==null || Id ==0)
            {
                return NotFound();
            }

            Producto producto = _db.Producto.Include(c=>c.Categoria)
                                            .Include(t=>t.TipoAplicacion)
                                            .FirstOrDefault(p=>p.Id==Id);

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

            _db.Producto.Remove(producto);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
