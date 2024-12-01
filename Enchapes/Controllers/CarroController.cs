using Enchapes_AccesoDatos.Datos;
using Enchapes_Modelos;
using Enchapes_Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Enchapes_Utilidades;

namespace Enchapes.Controllers
{

    [Authorize]
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }

        public CarroController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;

        }
        public IActionResult Index()
        {
            List<CarroCompra> carroCompraList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras)!=null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count()>0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> prodEnCarro = carroCompraList.Select(i=>i.ProductoId).ToList();  
            IEnumerable<Producto> prodList = _db.Producto.Where(p => prodEnCarro.Contains(p.Id));


            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Resumen));
        }

        public IActionResult Resumen()
        {
            //Traer usuario conectado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<CarroCompra> carroCompraList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> prodEnCarro = carroCompraList.Select(i => i.ProductoId).ToList();
            IEnumerable<Producto> prodList = _db.Producto.Where(p => prodEnCarro.Contains(p.Id));

            productoUsuarioVM = new ProductoUsuarioVM()
            {
                UsuarioAplicacion = _db.usuarioAplicacion.FirstOrDefault(u => u.Id == claim.Value),
                ProductoLista = prodList.ToList()
            };

            return View(productoUsuarioVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM)
        {
            var rutaTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                +"templates"+ Path.DirectorySeparatorChar.ToString()
                                + "PlantillaOrden.html";

            var subject = "Nueva Orden";
            string HtmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(rutaTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

        //Nombre: { 0}
        //Email: { 1}
        //Telefono: { 2}
        //Productos: { 3}

            StringBuilder productoListaSB = new StringBuilder();

            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                productoListaSB.Append($" - Nombre: {prod.NombreProducto} <span style='font-size:14px;'> (ID: {prod.Id}) </span> <br />");
            }

            string messageBody = string.Format(HtmlBody,
                                    productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                                    productoUsuarioVM.UsuarioAplicacion.Email,
                                    productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                                    productoListaSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin,subject, messageBody);

            return RedirectToAction(nameof(Confirmacion));
        }

        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remover(int Id)
        {
            List<CarroCompra> carroCompraList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            
            carroCompraList.Remove(carroCompraList.FirstOrDefault(p => p.ProductoId == Id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraList);

            return RedirectToAction(nameof(Index));
        }
    }
}
