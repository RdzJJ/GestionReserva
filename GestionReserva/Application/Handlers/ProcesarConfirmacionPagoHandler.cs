using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.Commands;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Handlers
{
    public class ProcesarConfirmacionPagoHandler : IRequestHandler<ProcesarConfirmacionPagoCommand, bool>
    {
        private readonly IReservaRepository _repo;
        public ProcesarConfirmacionPagoHandler(IReservaRepository repo) { _repo = repo; }

        public async Task<bool> Handle(ProcesarConfirmacionPagoCommand cmd, CancellationToken ct)
        {
            var reserva = await _repo.GetByIdAsync(new ReservaId(cmd.ReservaId), ct)
                          ?? throw new KeyNotFoundException($"Reserva {cmd.ReservaId} not found.");
            if (cmd.Exitoso)
                reserva.ProcesarConfirmacionPagoExterno(new PagoId(cmd.PagoIdInterno));
            _repo.Update(reserva);
            await _repo.UnitOfWork.SaveEntitiesAsync(ct);
            return true;
        }
    }
}