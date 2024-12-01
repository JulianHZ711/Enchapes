using Enchapes_AccesoDatos.Datos;
using Enchapes_Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Enchapes_Utilidades;


namespace Enchapes.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Categoria> lista = _db.Categoria;

            return View(lista);
        }

        //Get
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoria categoria)
        {

            if (ModelState.IsValid)
            {
                _db.Categoria.Add(categoria);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));

            }
            return View(categoria);
            
        }

        //Get editar
        public IActionResult Editar(int? Id)
        {
            if (Id == null || Id==0)
            {
                return NotFound();
            }
            var obj = _db.Categoria.Find(Id);
            if (obj == null)
            {
                return NotFound(); 
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {

            if (ModelState.IsValid)
            {
                _db.Categoria.Update(categoria);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));

            }
            return View(categoria);

        }

        //Get eliminar
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.Categoria.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoria categoria)
        {

            if (categoria ==null)
            {
                return NotFound();

            }
            _db.Categoria.Remove(categoria);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
            

        }
    }
}
