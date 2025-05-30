// File: Controllers/ProductosController.cs
using BackendApp.Models;
using BackendApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _service;

        public ProductosController(ProductoService service)
        {
            _service = service;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetAll()
        {
            var productos = await _service.GetAllAsync();
            return Ok(productos);
        }

        // GET: api/Productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetById(long id)
        {
            var producto = await _service.GetByIdAsync(id);
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }
    }
}
