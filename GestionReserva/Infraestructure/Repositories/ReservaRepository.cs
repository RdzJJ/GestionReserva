using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;
using GestionReserva.Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore; 
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic; 
using System.Linq; 

namespace GestionReserva.Infrastructure.Repositories
{
    // Implementación concreta de IReservaRepository usando Entity Framework Core.
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _context; // El DbContext inyectado.

        // Expone el DbContext como Unit of Work.
        public IUnitOfWork UnitOfWork => _context;

        // Constructor para inyección de dependencias.
        public ReservaRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Añade una nueva reserva al DbSet en el DbContext.
        public async Task AddAsync(Reserva reserva, CancellationToken cancellationToken = default)
        {
            await _context.Reservas.AddAsync(reserva, cancellationToken);
        }

        // Obtiene una reserva por ID, asegurándose de cargar todas las entidades contenidas
        // necesarias para reconstituir el agregado completo (Eager Loading con Include/ThenInclude).
        public async Task<Reserva?> GetByIdAsync(ReservaId id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Include(r => r.Oferta) // Carga la oferta
                    .ThenInclude(o => o.DetallesServicio) // Carga los detalles dentro de la oferta
                .Include(r => r.Pagos) // Carga la colección de pagos
                .Include(r => r.Voucher) // Carga el voucher (si existe)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken); // Busca por ID.
        }

        // Elimina una reserva del DbContext.
        public void Remove(Reserva reserva)
        {
            _context.Reservas.Remove(reserva);
        }

        // Marca una entidad como modificada en el DbContext (a menudo no es necesario si se obtuvo del mismo contexto).
        public void Update(Reserva reserva)
        {
            _context.Entry(reserva).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Reserva>> FindByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Oferta).ThenInclude(o => o.DetallesServicio)
                .Include(r => r.Pagos)
                .Include(r => r.Voucher)
                .ToListAsync(cancellationToken);
        }
    }
}