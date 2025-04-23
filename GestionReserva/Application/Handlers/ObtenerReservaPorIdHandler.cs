using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.DTOs;
using GestionReserva.Application.Queries;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Handlers
{
    public class ObtenerReservaPorIdHandler : IRequestHandler<ObtenerReservaPorIdQuery, ReservaDto>
    {
        private readonly IReservaRepository _repo;
        public ObtenerReservaPorIdHandler(IReservaRepository repo) { _repo = repo; }

        public async Task<ReservaDto> Handle(ObtenerReservaPorIdQuery qry, CancellationToken ct)
        {
            var r = await _repo.GetByIdAsync(new ReservaId(qry.Id), ct);
            if (r == null) return null;

            var dto = new ReservaDto
            {
                Id = r.Id.Value,
                UsuarioId = r.UsuarioId,
                Oferta = new OfertaDto
                {
                    Destino = new DestinoDto { Nombre = r.Oferta.Destino.Nombre, Pais = r.Oferta.Destino.Pais },
                    Fechas = new FechasDeViajeDto { Inicio = r.Oferta.Fechas.Inicio, Fin = r.Oferta.Fechas.Fin },
                    DetallesServicio = r.Oferta.DetallesServicio.Select(d => new DetalleServicioDto
                    {
                        Tipo = d.Tipo,
                        Descripcion = d.Descripcion,
                        Precio = new MontoDto { Valor = d.Precio.Valor, Moneda = d.Precio.Moneda },
                        ConfirmadoExternamente = d.ConfirmadoExternamente
                    }),
                    CostoTotal = new MontoDto { Valor = r.Oferta.CalcularCostoTotal().Valor, Moneda = r.Oferta.CalcularCostoTotal().Moneda }
                },
                Pagos = r.Pagos.Select(p => new PagoDto
                {
                    Id = p.Id.Value,
                    MontoPagado = new MontoDto { Valor = p.MontoPagado.Valor, Moneda = p.MontoPagado.Moneda },
                    Tipo = p.Tipo,
                    FechaPago = p.FechaPago,
                    ConfirmadoExternamente = p.ConfirmadoExternamente
                }),
                Voucher = r.Voucher == null ? null : new VoucherDto
                {
                    Id = r.Voucher.Id.Value,
                    Codigo = r.Voucher.Codigo,
                    FechaEmision = r.Voucher.FechaEmision
                },
                Estado = r.Estado.ToString(),
                SaldoPendiente = new MontoDto { Valor = r.CalcularSaldoPendiente().Valor, Moneda = r.CalcularSaldoPendiente().Moneda }
            };

            return dto;
        }
    }
}
