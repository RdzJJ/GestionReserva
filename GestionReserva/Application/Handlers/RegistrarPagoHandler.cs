using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.Commands;
using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Handlers
{
    public class RegistrarPagoHandler : IRequestHandler<RegistrarPagoCommand, Guid>
    {
        private readonly IReservaRepository _repo;
        public RegistrarPagoHandler(IReservaRepository repo) { _repo = repo; }

        public async Task<Guid> Handle(RegistrarPagoCommand cmd, CancellationToken ct)
        {
            var reserva = await _repo.GetByIdAsync(new ReservaId(cmd.ReservaId), ct)
                          ?? throw new KeyNotFoundException($"Reserva {cmd.ReservaId} not found.");
            var pagoId = new PagoId(Guid.NewGuid());
            var pago = new Pago(pagoId, new Monto(cmd.NuevoPago.MontoPagado.Valor, cmd.NuevoPago.MontoPagado.Moneda), cmd.NuevoPago.Tipo);
            reserva.AgregarPago(pago);
            _repo.Update(reserva);
            await _repo.UnitOfWork.SaveEntitiesAsync(ct);
            return pagoId.Value;
        }
    }
}