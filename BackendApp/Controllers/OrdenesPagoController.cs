// File: Controllers/OrdenesPagoController.cs
using BackendApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetOrdenesPago()
        {
            var ordenes = await _ordenPagoService.GetOrdenesPagoAsync();
            return Ok(ordenes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(long id)
        {
            var deleted = await _ordenPagoService.DeleteOrdenAsync(id);
            if (!deleted)
                return NotFound($"No se encontró la orden con id {id}");

            return NoContent();
        }
    }
}
