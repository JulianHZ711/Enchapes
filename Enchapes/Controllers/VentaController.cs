using Enchapes_AccesoDatos.Data.Repositorio.IRepositorio;
using Enchapes_Modelos.ViewModels;
using Enchapes_Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enchapes.Controllers
{
    public class VentaController : Controller
    {

        private readonly IVentaRepositorio _ventaRepo;
        private readonly IVentaDetalleRepositorio _ventaDetalleRepo;

        public VentaController(IVentaRepositorio ventaRepo, IVentaDetalleRepositorio ventaDetalleRepo)
        {
            _ventaRepo = ventaRepo;
            _ventaDetalleRepo = ventaDetalleRepo;
        }
        public IActionResult Index(string buscarNombre = null, string buscarEmail = null, string buscarTelefono = null, string Estado = null)
        {
            VentaVM ventaVM = new VentaVM()
            {
                VentaLista = _ventaRepo.ObtenerTodos(),
                EstadoLista = WC.ListaEstados.ToList().Select(l => new SelectListItem
                {
                    Text = l,
                    Value = l
                })
            };

            if (!string.IsNullOrEmpty(buscarNombre))
            {
                ventaVM.VentaLista = ventaVM.VentaLista.Where(u => u.NombreCompleto.ToLower().Contains(buscarNombre.ToLower()));
            }

            if (!string.IsNullOrEmpty(buscarEmail))
            {
                ventaVM.VentaLista = ventaVM.VentaLista.Where(u => u.Email.ToLower().Contains(buscarEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(buscarTelefono))
            {
                ventaVM.VentaLista = ventaVM.VentaLista.Where(u => u.Telefono.ToLower().Contains(buscarTelefono.ToLower()));
            }

            if (!string.IsNullOrEmpty(Estado) && Estado != "--Estado--")
            {
                ventaVM.VentaLista = ventaVM.VentaLista.Where(u => u.EstadoVenta.ToLower().Contains(Estado.ToLower()));
            }

            return View(ventaVM);
        }
    }
}
