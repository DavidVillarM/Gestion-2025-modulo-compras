//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ModuloCompras.Data;
//using ModuloCompras.DTOs;
//using ModuloCompras.Models;

//namespace ModuloCompras.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProveedoresController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        public ProveedoresController(ApplicationDbContext context) => _context = context;

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ProveedorReadDto>>> GetAll()
//        {
//            var list = await _context.Proveedores
//                .Include(p => p.CategoriaProveedores)
//                  .ThenInclude(cp => cp.Categoria)
//                .ToListAsync();

//            var dtos = list.Select(p => new ProveedorReadDto
//            {
//                IdProveedor = p.IdProveedor,
//                Ruc = p.Ruc,
//                Nombre = p.Nombre,
//                Telefono = p.Telefono,
//                Correo = p.Correo,
//                NombreContacto = p.NombreContacto,
//                Categorias = p.CategoriaProveedores
//                                 .Select(cp => new CategoriaAsignadaDto
//                                 {
//                                     IdCategoria = cp.IdCategoria,
//                                     Nombre = cp.Categoria.Nombre,
//                                     Descripcion = cp.Categoria.Descripcion,
//                                     Estado = cp.Estado
//                                 }).ToList()
//            });

//            return Ok(dtos);
//        }
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ProveedorReadDto>> Get(int id)
//        {
//            var p = await _context.Proveedores
//                .Include(x => x.CategoriaProveedores)
//                  .ThenInclude(cp => cp.Categoria)
//                .FirstOrDefaultAsync(x => x.IdProveedor == id);
//            if (p == null) return NotFound();

//            var dto = new ProveedorReadDto
//            {
//                IdProveedor = p.IdProveedor,
//                Ruc = p.Ruc,
//                Nombre = p.Nombre,
//                Telefono = p.Telefono,
//                Correo = p.Correo,
//                NombreContacto = p.NombreContacto,
//                Categorias = p.CategoriaProveedores
//                                   .Select(cp => new CategoriaAsignadaDto
//                                   {
//                                       IdCategoria = cp.IdCategoria,
//                                       Nombre = cp.Categoria.Nombre,
//                                       Descripcion = cp.Categoria.Descripcion,
//                                       Estado = cp.Estado
//                                   }).ToList()
//            };
//            return Ok(dto);
//        }

//        [HttpPost]
//        public async Task<ActionResult<ProveedorReadDto>> Create([FromBody] ProveedorCreateDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var prov = new Proveedor
//            {
//                Ruc = dto.Ruc,
//                Nombre = dto.Nombre,
//                Telefono = dto.Telefono,
//                Correo = dto.Correo,
//                NombreContacto = dto.NombreContacto
//            };
//            _context.Proveedores.Add(prov);
//            await _context.SaveChangesAsync();

//            if (dto.CategoriaIds != null && dto.CategoriaIds.Any())
//            {
//                foreach (var catId in dto.CategoriaIds.Distinct())
//                {
//                    if (!await _context.Categorias.AnyAsync(c => c.IdCategoria == catId))
//                        return BadRequest($"Categoría {catId} no existe.");

//                    prov.CategoriaProveedores.Add(new CategoriaProveedor
//                    {
//                        IdProveedor = prov.IdProveedor,
//                        IdCategoria = catId,
//                        Estado = "ACTIVO"
//                    });
//                }
//                await _context.SaveChangesAsync();
//            }

//            // Cargar navegación de categorías
//            await _context.Entry(prov)
//                .Collection(p => p.CategoriaProveedores)
//                .Query()
//                .Include(cp => cp.Categoria)
//                .LoadAsync();

//            var result = new ProveedorReadDto
//            {
//                IdProveedor = prov.IdProveedor,
//                Ruc = prov.Ruc,
//                Nombre = prov.Nombre,
//                Telefono = prov.Telefono,
//                Correo = prov.Correo,
//                NombreContacto = prov.NombreContacto,
//                Categorias = prov.CategoriaProveedores
//                                 .Select(cp => new CategoriaAsignadaDto
//                                 {
//                                     IdCategoria = cp.IdCategoria,
//                                     Nombre = cp.Categoria.Nombre,
//                                     Descripcion = cp.Categoria.Descripcion,
//                                     Estado = cp.Estado
//                                 })
//                                 .ToList()
//            };

//            return CreatedAtAction(nameof(Get), new { id = prov.IdProveedor }, result);
//        }

//        // PUT: api/proveedores/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] ProveedorCreateDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            var prov = await _context.Proveedores
//                .Include(p => p.CategoriaProveedores)
//                .FirstOrDefaultAsync(p => p.IdProveedor == id);
//            if (prov == null) return NotFound();

//            prov.Ruc = dto.Ruc;
//            prov.Nombre = dto.Nombre;
//            prov.Telefono = dto.Telefono;
//            prov.Correo = dto.Correo;
//            prov.NombreContacto = dto.NombreContacto;

//            // Actualizar categorías: eliminar no seleccionadas y añadir nuevas
//            var newIds = dto.CategoriaIds?.Distinct().ToList() ?? new List<int>();
//            var toRemove = prov.CategoriaProveedores
//                .Where(cp => !newIds.Contains(cp.IdCategoria))
//                .ToList();
//            _context.CategoriaProveedores.RemoveRange(toRemove);

//            foreach (var catId in newIds)
//            {
//                if (!prov.CategoriaProveedores.Any(cp => cp.IdCategoria == catId))
//                {
//                    if (!await _context.Categorias.AnyAsync(c => c.IdCategoria == catId))
//                        return BadRequest($"Categoría {catId} no existe.");
//                    prov.CategoriaProveedores.Add(new CategoriaProveedor
//                    {
//                        IdProveedor = id,
//                        IdCategoria = catId,
//                        Estado = "ACTIVO"
//                    });
//                }
//            }

//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        // DELETE: api/proveedores/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var prov = await _context.Proveedores
//                .Include(p => p.CategoriaProveedores)
//                .FirstOrDefaultAsync(p => p.IdProveedor == id);
//            if (prov == null) return NotFound();

//            _context.CategoriaProveedores.RemoveRange(prov.CategoriaProveedores);
//            _context.Proveedores.Remove(prov);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Data;
using ModuloCompras.Models;
using ModuloCompras.DTOs;

namespace ModuloCompras.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProveedoresController(ApplicationDbContext context) => _context = context;

        // ─── GET: api/proveedores ───
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorReadDto>>> GetAll()
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.CategoriaProveedores)
                    .ThenInclude(cp => cp.Categoria)
                .ToListAsync();

            var dtos = proveedores.Select(p => new ProveedorReadDto
            {
                IdProveedor = p.IdProveedor,
                Ruc = p.Ruc,
                Nombre = p.Nombre,
                Telefono = p.Telefono,
                Correo = p.Correo,
                NombreContacto = p.NombreContacto,
                Categorias = p.CategoriaProveedores
                                .Where(cp => cp.Estado == "ACTIVO")
                                .Select(cp => new CategoriaAsignadaDto
                                {
                                    IdCategoria = cp.IdCategoria,
                                    Nombre = cp.Categoria.Nombre,
                                    Descripcion = cp.Categoria.Descripcion,
                                    Estado = cp.Estado
                                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        // ─── GET: api/proveedores/{id} ───
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorReadDto>> Get(int id)
        {
            var p = await _context.Proveedores
                .Include(p => p.CategoriaProveedores)
                    .ThenInclude(cp => cp.Categoria)
                .FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (p == null)
                return NotFound(new { Message = $"Proveedor {id} no existe." });

            var dto = new ProveedorReadDto
            {
                IdProveedor = p.IdProveedor,
                Ruc = p.Ruc,
                Nombre = p.Nombre,
                Telefono = p.Telefono,
                Correo = p.Correo,
                NombreContacto = p.NombreContacto,
                Categorias = p.CategoriaProveedores
                                .Where(cp => cp.Estado == "ACTIVO")
                                .Select(cp => new CategoriaAsignadaDto
                                {
                                    IdCategoria = cp.IdCategoria,
                                    Nombre = cp.Categoria.Nombre,
                                    Descripcion = cp.Categoria.Descripcion,
                                    Estado = cp.Estado
                                }).ToList()
            };
            return Ok(dto);
        }

        // ─── POST: api/proveedores ───
        [HttpPost]
        public async Task<ActionResult<ProveedorReadDto>> Create([FromBody] ProveedorCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var prov = new Proveedor
            {
                Ruc = dto.Ruc,
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                NombreContacto = dto.NombreContacto
            };

            _context.Proveedores.Add(prov);
            await _context.SaveChangesAsync();

            if (dto.CategoriaIds != null && dto.CategoriaIds.Any())
            {
                var distinctIds = dto.CategoriaIds.Distinct().ToList();
                var existingCats = await _context.Categorias
                    .Where(c => distinctIds.Contains(c.IdCategoria))
                    .Select(c => c.IdCategoria)
                    .ToListAsync();
                var missing = distinctIds.Except(existingCats).ToList();
                if (missing.Any())
                    return BadRequest(new { Message = $"Categorías no existentes: {string.Join(", ", missing)}" });

                foreach (var catId in distinctIds)
                {
                    prov.CategoriaProveedores.Add(new CategoriaProveedor
                    {
                        IdProveedor = prov.IdProveedor,
                        IdCategoria = catId,
                        Estado = "ACTIVO"
                    });
                }
                await _context.SaveChangesAsync();
            }

            await _context.Entry(prov)
                .Collection(p => p.CategoriaProveedores)
                .Query()
                .Include(cp => cp.Categoria)
                .LoadAsync();

            var result = new ProveedorReadDto
            {
                IdProveedor = prov.IdProveedor,
                Ruc = prov.Ruc,
                Nombre = prov.Nombre,
                Telefono = prov.Telefono,
                Correo = prov.Correo,
                NombreContacto = prov.NombreContacto,
                Categorias = prov.CategoriaProveedores
                                .Where(cp => cp.Estado == "ACTIVO")
                                .Select(cp => new CategoriaAsignadaDto
                                {
                                    IdCategoria = cp.IdCategoria,
                                    Nombre = cp.Categoria.Nombre,
                                    Descripcion = cp.Categoria.Descripcion,
                                    Estado = cp.Estado
                                }).ToList()
            };

            return CreatedAtAction(nameof(Get), new { id = prov.IdProveedor }, result);
        }

        // ─── PUT: api/proveedores/{id} ───
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProveedorCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var prov = await _context.Proveedores
                .Include(p => p.CategoriaProveedores)
                .FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (prov == null)
                return NotFound(new { Message = $"Proveedor {id} no existe." });

            prov.Ruc = dto.Ruc;
            prov.Nombre = dto.Nombre;
            prov.Telefono = dto.Telefono;
            prov.Correo = dto.Correo;
            prov.NombreContacto = dto.NombreContacto;

            var newIds = dto.CategoriaIds?.Distinct().ToList() ?? new List<int>();

            var toRemove = prov.CategoriaProveedores
                .Where(cp => !newIds.Contains(cp.IdCategoria))
                .ToList();
            _context.CategoriaProveedores.RemoveRange(toRemove);

            foreach (var catId in newIds)
            {
                if (!prov.CategoriaProveedores.Any(cp => cp.IdCategoria == catId))
                {
                    var existsCat = await _context.Categorias.AnyAsync(c => c.IdCategoria == catId);
                    if (!existsCat)
                        return BadRequest(new { Message = $"Categoría {catId} no existe." });

                    prov.CategoriaProveedores.Add(new CategoriaProveedor
                    {
                        IdProveedor = id,
                        IdCategoria = catId,
                        Estado = "ACTIVO"
                    });
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ─── DELETE: api/proveedores/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prov = await _context.Proveedores
                .Include(p => p.CategoriaProveedores)
                .FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (prov == null)
                return NotFound(new { Message = $"Proveedor {id} no existe." });

            _context.CategoriaProveedores.RemoveRange(prov.CategoriaProveedores);
            _context.Proveedores.Remove(prov);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
