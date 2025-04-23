using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Persistence
{
    public class ReservaDbContext : DbContext
    {
        public ReservaDbContext(DbContextOptions<ReservaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ServicioReservado> ServiciosReservados { get; set; }
    }
}