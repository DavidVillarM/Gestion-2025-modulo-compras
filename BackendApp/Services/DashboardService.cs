using BackendApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Services
{
    public class DashboardService
    {
        private readonly PostgresContext _context;

        public DashboardService(PostgresContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalComprasDelMes()
        {
            var inicioMes = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.Facturas
                .Where(f => f.Fecha >= inicioMes)
                .SumAsync(f => f.MontoTotal ?? 0);
        }

        public async Task<int> GetOrdenesPendientes()
        {
            return await _context.Ordenes.CountAsync(o => o.Estado == "Pendiente");
        }

        public async Task<int> GetPedidosEnCurso()
        {
            return await _context.Pedidos.CountAsync(p => p.Estado == "En curso");
        }

        public async Task<int> GetPagosPendientes()
        {
            return await _context.Facturas.CountAsync(f => f.Estado == "Pendiente");
        }

        public async Task<List<object>> GetProductosMasPedidos()
        {
            return await _context.OrdenDetalles
                .GroupBy(d => d.IdProducto)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    Total = g.Sum(x => x.Cantidad ?? 0)
                })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .Join(_context.Productos, x => x.ProductoId, p => p.IdProducto, (x, p) => new { x.Total, p.Nombre, p.IdProducto })
                .GroupJoin(_context.Precios.OrderByDescending(p => p.FechaRegistro),
                    prod => prod.IdProducto,
                    precio => precio.IdProducto,
                    (prod, precios) => new { prod.Nombre, prod.Total, Precio = precios.FirstOrDefault() })
                .Select(x => new
                {
                    nombre = x.Nombre,
                    total = x.Total,
                    precioUnitario = x.Precio != null ? x.Precio.Precio1 : 0
                })
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetProductosMenosPedidos()
        {
            return await _context.OrdenDetalles
                .GroupBy(d => d.IdProducto)
                .Select(g => new
                {
                    ProductoId = g.Key,
                    Total = g.Sum(x => x.Cantidad ?? 0)
                })
                .OrderBy(x => x.Total)
                .Take(10)
                .Join(_context.Productos, x => x.ProductoId, p => p.IdProducto, (x, p) => new { x.Total, p.Nombre, p.IdProducto })
                .GroupJoin(_context.Precios.OrderByDescending(p => p.FechaRegistro),
                    prod => prod.IdProducto,
                    precio => precio.IdProducto,
                    (prod, precios) => new { prod.Nombre, prod.Total, Precio = precios.FirstOrDefault() })
                .Select(x => new
                {
                    nombre = x.Nombre,
                    total = x.Total,
                    precioUnitario = x.Precio != null ? x.Precio.Precio1 : 0
                })
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetProductosPedidosPorMes()
        {
            var datos = await _context.OrdenDetalles
                .Where(d => d.IdOrdenNavigation.Pedidos.Any(p => p.FechaPedido.HasValue))
                .SelectMany(d => d.IdOrdenNavigation.Pedidos
                    .Where(p => p.FechaPedido.HasValue)
                    .Select(p => new
                    {
                        Mes = new DateTime(p.FechaPedido.Value.Year, p.FechaPedido.Value.Month, 1),
                        Cantidad = d.Cantidad ?? 0
                    }))
                .ToListAsync();

            return datos
                .GroupBy(x => x.Mes)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Mes = g.Key.ToString("yyyy-MM", CultureInfo.InvariantCulture),
                    Total = g.Sum(x => x.Cantidad)
                })
                .Cast<object>()
                .ToList();
        }
    }
}
