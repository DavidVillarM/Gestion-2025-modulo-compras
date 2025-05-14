// File: Services/OrdenPagoService.cs
using BackendApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Services
{
    public class OrdenPagoService
    {
        private readonly PostgresContext _context;

        public OrdenPagoService(PostgresContext context)
        {
            _context = context;
        }

        public async Task<List<object>> GetOrdenesPagoAsync()
        {
            var query = _context.Ordenes
                .Select(o => new
                {
                    Id = o.IdOrden,
                    NroOrden = "OP-" + o.IdOrden.ToString("D3"),
                    Estado = o.Estado,
                    Total = o.Pedidos.Sum(p => p.MontoTotal ?? 0m),
                    Fecha = o.Pedidos
                                     .Where(p => p.FechaPedido.HasValue)
                                     .OrderByDescending(p => p.FechaPedido)
                                     .Select(p => p.FechaPedido)
                                     .FirstOrDefault()
                });

            return await query.Cast<object>().ToListAsync();
        }

        /// <summary>
        /// Elimina todos los pedidos de la orden especificada.
        /// Luego, si la orden queda sin pedidos, también la elimina.
        /// </summary>
        public async Task<bool> DeleteOrdenAsync(long idOrden)
        {
            var orden = await _context.Ordenes
                .Include(o => o.Pedidos)
                .FirstOrDefaultAsync(o => o.IdOrden == idOrden);

            if (orden == null)
                return false;

            // Eliminar pedidos si existen
            if (orden.Pedidos.Any())
            {
                _context.Pedidos.RemoveRange(orden.Pedidos);
                await _context.SaveChangesAsync(); // ✅ Guardar eliminación de pedidos antes
            }

            // Verificar si aún quedan pedidos en la base de datos
            var quedanPedidos = await _context.Pedidos.AnyAsync(p => p.IdOrden == idOrden);

            // Si no quedan, eliminar la orden
            if (!quedanPedidos)
            {
                _context.Ordenes.Remove(orden);
                await _context.SaveChangesAsync(); // ✅ Guardar eliminación de orden
            }

            return true;
        }

    }
}
