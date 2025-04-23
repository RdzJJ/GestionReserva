using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Interfaces;
using Core.Aggregates;

namespace Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly List<Reserva> _reservas = new();

        public void Add(Reserva reserva) => _reservas.Add(reserva);
        public void Update(Reserva reserva) { /* Actualiza la reserva */ }
        public Reserva? GetById(int id) => _reservas.FirstOrDefault(r => r.Id == id);
    }
}