using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Aggregates;
using Core.ValueObjects;

namespace Infrastructure.Persistence
{
    public class ReservaDbContext : DbContext
    {
        public ReservaDbContext(DbContextOptions<ReservaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<DetalleServicio> DetalleServicio { get; set; }
    }
}