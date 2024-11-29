using Enchapes.Data;
using Enchapes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Enchapes_Utilidades;

namespace Enchapes.Controllers

{

    [Authorize(Roles = WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TipoAplicacionController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _db.TipoAplicacion;

            return View(lista);
        }

        //Get
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {

            if (ModelState.IsValid)
            {
                _db.TipoAplicacion.Add(tipoAplicacion);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));

            }
            return View(tipoAplicacion);

        }

        //Get editar
        public IActionResult Editar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.TipoAplicacion.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {

            if (ModelState.IsValid)
            {
                _db.TipoAplicacion.Update(tipoAplicacion);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));

            }
            return View(tipoAplicacion);

        }

        //Get eliminar
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = _db.TipoAplicacion.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion)
        {

            if (tipoAplicacion == null)
            {
                return NotFound();

            }
            _db.TipoAplicacion.Remove(tipoAplicacion);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));


        }


    }
}
