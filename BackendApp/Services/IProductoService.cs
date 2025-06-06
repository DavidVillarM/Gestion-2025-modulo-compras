using BackendApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApp.Services
{
    public class ProductoService
    {
        private readonly PostgresContext _context;

        public ProductoService(PostgresContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(long id)
        {
            return await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProducto == id);
        }
    }
}
