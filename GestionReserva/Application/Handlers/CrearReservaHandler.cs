using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.Commands;
using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Handlers
{
    public class CrearReservaHandler : IRequestHandler<CrearReservaCommand, Guid>
    {
        private readonly IReservaRepository _repo;
        public CrearReservaHandler(IReservaRepository repo) { _repo = repo; }

        public async Task<Guid> Handle(CrearReservaCommand cmd, CancellationToken ct)
        {
            // Mapeo VOs
            var destino = new Destino(cmd.Destino.Nombre, cmd.Destino.Pais);
            var fechas = new FechasDeViaje(cmd.Fechas.Inicio, cmd.Fechas.Fin);
            var detalles = cmd.DetallesServicio.Select(d => new DetalleServicio(d.Tipo, d.Descripcion, new Monto(d.Precio.Valor, d.Precio.Moneda))).ToList();
            var oferta = new OfertaPersonalizada(destino, fechas, detalles);

            // Pago inicial
            var pagoId = new PagoId(Guid.NewGuid());
            var pagoInicial = new Pago(pagoId, new Monto(cmd.MontoInicial.Valor, cmd.MontoInicial.Moneda), cmd.TipoPago);

            // Crear reserva
            var reserva = Reserva.CrearNueva(new ReservaId(Guid.NewGuid()), cmd.UsuarioId, oferta, pagoInicial);
            await _repo.AddAsync(reserva, ct);
            await _repo.UnitOfWork.SaveEntitiesAsync(ct);
            return reserva.Id.Value;
        }
    }
}