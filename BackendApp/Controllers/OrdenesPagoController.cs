
// File: Controllers/OrdenesPagoController.cs
using BackendApp.Services;
using BackendApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BackendApp.Controllers
{
    [ApiController]
    
    [Route("api/[controller]")]
    public class OrdenesPagoController : ControllerBase
    {
        private readonly OrdenPagoService _ordenPagoService;

        public OrdenesPagoController(OrdenPagoService ordenPagoService)
        {
            _ordenPagoService = ordenPagoService;
        }

        // GET api/OrdenesPago
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<OrdenPagoDto>>> GetOrdenesPago()
        {
            var ordenes = await _ordenPagoService.GetOrdenesPagoAsync();
            return Ok(ordenes);
        }

        // GET api/OrdenesPago/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenPagoDto>> GetOrdenPagoById(long id)
        {
            var orden = (await _ordenPagoService.GetOrdenesPagoAsync())
                           .FirstOrDefault(o => o.IdOrden == id);
            if (orden == null)
                return NotFound();
            return Ok(orden);
        }

        // GET api/OrdenesPago/{id}/detalles
        [HttpGet("{id}/detalles")]
        public async Task<ActionResult<List<OrdenDetalle>>> GetOrdenDetalles(long id)
        {
            var detalles = await _ordenPagoService.GetDetallesByOrdenAsync(id);
            if (detalles == null || detalles.Count == 0)
                return NotFound();
            return Ok(detalles);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearPedidos([FromBody] CrearPedidosDto dto)
        {
            var resultado = await _ordenPagoService.CreatePedidosAsync(dto);
            if (!resultado)
                return BadRequest("No se encontró la orden o ocurrió un error.");

            return Ok(new { mensaje = "Pedidos creados correctamente." });
        }

        [HttpGet("{id}/pedidos-con-detalles")]
        public async Task<ActionResult<List<PedidoConDetallesDto>>> GetPedidosConDetalles(long id)
        {
            var resultado = await _ordenPagoService.GetPedidosConDetallesByOrdenAsync(id);
            if (resultado == null || resultado.Count == 0)
                return NotFound();
            return Ok(resultado);
        }

        // DELETE api/OrdenesPago/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(long id)
        {
            var deleted = await _ordenPagoService.DeleteOrdenAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
