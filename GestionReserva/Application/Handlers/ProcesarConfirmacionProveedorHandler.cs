using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.Commands;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Handlers
{
    public class ProcesarConfirmacionProveedorHandler : IRequestHandler<ProcesarConfirmacionProveedorCommand, bool>
    {
        private readonly IReservaRepository _repo;
        public ProcesarConfirmacionProveedorHandler(IReservaRepository repo) { _repo = repo; }

        public async Task<bool> Handle(ProcesarConfirmacionProveedorCommand cmd, CancellationToken ct)
        {
            var reserva = await _repo.GetByIdAsync(new ReservaId(cmd.ReservaId), ct)
                          ?? throw new KeyNotFoundException($"Reserva {cmd.ReservaId} not found.");
            if (cmd.Exitoso)
                reserva.ProcesarConfirmacionServicioExterno(cmd.TipoServicio, cmd.DescripcionServicio);
            _repo.Update(reserva);
            await _repo.UnitOfWork.SaveEntitiesAsync(ct);
            return true;
        }
    }
}