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
            // 1) Traer la orden junto con sus pedidos
            var orden = await _context.Ordenes
                .Include(o => o.Pedidos)
                .FirstOrDefaultAsync(o => o.IdOrden == idOrden);

            if (orden == null)
                return false;  // no existe

            // 2) Eliminar pedidos asociados
            if (orden.Pedidos.Any())
            {
                _context.Pedidos.RemoveRange(orden.Pedidos);
            }

            // 3) Comprobar de nuevo si hay pedidos (por seguridad)
            var quedanPedidos = await _context.Pedidos.AnyAsync(p => p.IdOrden == idOrden);

            // 4) Si ya no hay pedidos, eliminar la orden
            if (!quedanPedidos)
            {
                _context.Ordenes.Remove(orden);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
