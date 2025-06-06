// File: Controllers/PresupuestosController.cs
using BackendApp.Services;
using BackendApp.Dtos;
using BackendApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresupuestooController : ControllerBase
    {
        private readonly PresupuestoService _presupuestoService;

        public PresupuestooController(PresupuestoService presupuestoService)
        {
            _presupuestoService = presupuestoService;
        }

        /// <summary>
        /// GET api/Presupuestos/Orden/{idOrden}
        /// Devuelve todos los presupuestos (con detalles) de la orden indicada.
        /// </summary>
        [HttpGet("Orden/{idOrden}")]
        public async Task<ActionResult<List<PresupuestoConDetallesDto>>> GetByOrden(long idOrden)
        {
            var lista = await _presupuestoService.GetPresupuestosByOrdenAsync(idOrden);
            if (lista == null || lista.Count == 0)
                return NotFound();   // 404 si no existe o no tiene presupuestos

            return Ok(lista);
        }
    }
}
