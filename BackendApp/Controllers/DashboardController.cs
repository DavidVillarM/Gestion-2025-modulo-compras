using Microsoft.AspNetCore.Mvc;
using BackendApp.Services;
using System.Threading.Tasks;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("compras-mensuales")]
        public async Task<IActionResult> GetComprasMensuales()
        {
            var total = await _dashboardService.GetTotalComprasDelMes();
            return Ok(total);
        }

        [HttpGet("ordenes-pendientes")]
        public async Task<IActionResult> GetOrdenesPendientes()
        {
            var total = await _dashboardService.GetOrdenesPendientes();
            return Ok(total);
        }

        [HttpGet("pedidos-en-curso")]
        public async Task<IActionResult> GetPedidosEnCurso()
        {
            var total = await _dashboardService.GetPedidosEnCurso();
            return Ok(total);
        }

        [HttpGet("pagos-pendientes")]
        public async Task<IActionResult> GetPagosPendientes()
        {
            var total = await _dashboardService.GetPagosPendientes();
            return Ok(total);
        }

        [HttpGet("productos-mas-pedidos")]
        public async Task<IActionResult> GetProductosMasPedidos()
        {
            var productos = await _dashboardService.GetProductosMasPedidos();
            return Ok(productos);
        }

        [HttpGet("productos-menos-pedidos")]
        public async Task<IActionResult> GetProductosMenosPedidos()
        {
            var productos = await _dashboardService.GetProductosMenosPedidos();
            return Ok(productos);
        }

        [HttpGet("productos-por-mes")]
        public async Task<IActionResult> GetProductosPorMes()
        {
            var datos = await _dashboardService.GetProductosPedidosPorMes();
            return Ok(datos);
        }
    }
}
