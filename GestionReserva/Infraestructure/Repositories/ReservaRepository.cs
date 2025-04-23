using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly List<ReservaController> _reservas = new();

        public void Add(ReservaController reserva) => _reservas.Add(reserva);
        public void Update(ReservaController reserva) { /* Actualiza la reserva */ }
        public ReservaController GetById(int id) => _reservas.FirstOrDefault(r => r.Id == id);
    }
}