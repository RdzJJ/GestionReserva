using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.Commands;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Handlers
{
    public class CancelarReservaHandler : IRequestHandler<CancelarReservaCommand, bool>
    {
        private readonly IReservaRepository _repo;
        public CancelarReservaHandler(IReservaRepository repo) { _repo = repo; }

        public async Task<bool> Handle(CancelarReservaCommand cmd, CancellationToken ct)
        {
            var reserva = await _repo.GetByIdAsync(new ReservaId(cmd.ReservaId), ct)
                          ?? throw new KeyNotFoundException($"Reserva {cmd.ReservaId} not found.");
            reserva.Cancelar();
            _repo.Update(reserva);
            await _repo.UnitOfWork.SaveEntitiesAsync(ct);
            return true;
        }
    }
}