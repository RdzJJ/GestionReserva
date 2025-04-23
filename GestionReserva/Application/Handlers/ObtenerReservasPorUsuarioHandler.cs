using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestionReserva.Application.DTOs;
using GestionReserva.Application.Queries;
using GestionReserva.Core.Interfaces;

namespace GestionReserva.Application.Handlers
{
    public class ObtenerReservasPorUsuarioHandler
        : IRequestHandler<ObtenerReservasPorUsuarioQuery, List<ReservaDto>>
    {
        private readonly IReservaRepository _repository;
        public ObtenerReservasPorUsuarioHandler(IReservaRepository repository)
            => _repository = repository;

        public async Task<List<ReservaDto>> Handle(ObtenerReservasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var reservas = await _repository.FindByUsuarioIdAsync(request.UsuarioId, cancellationToken);

            return reservas.Select(r => new ReservaDto
            {
                Id = r.Id.Value,
                UsuarioId = r.UsuarioId,
                Oferta = new OfertaDto
                {
                    Destino = new DestinoDto
                    {
                        Nombre = r.Oferta.Destino.Nombre,
                        Pais = r.Oferta.Destino.Pais
                    },
                    Fechas = new FechasDeViajeDto
                    {
                        Inicio = r.Oferta.Fechas.Inicio,
                        Fin = r.Oferta.Fechas.Fin
                    },
                    DetallesServicio = r.Oferta.DetallesServicio
                        .Select(ds => new DetalleServicioDto
                        {
                            Tipo = ds.Tipo,
                            Descripcion = ds.Descripcion,
                            Precio = new MontoDto
                            {
                                Valor = ds.Precio.Valor,
                                Moneda = ds.Precio.Moneda
                            },
                            ConfirmadoExternamente = ds.ConfirmadoExternamente
                        })
                        .ToList(),
                    CostoTotal = new MontoDto
                    {
                        Valor = r.Oferta.CalcularCostoTotal().Valor,
                        Moneda = r.Oferta.CalcularCostoTotal().Moneda
                    }
                },
                Pagos = r.Pagos.Select(p => new PagoDto
                {
                    Id = p.Id.Value,
                    MontoPagado = new MontoDto
                    {
                        Valor = p.MontoPagado.Valor,
                        Moneda = p.MontoPagado.Moneda
                    },
                    Tipo = p.Tipo,
                    FechaPago = p.FechaPago,
                    ConfirmadoExternamente = p.ConfirmadoExternamente
                }).ToList(),
                Voucher = r.Voucher is null ? null : new VoucherDto
                {
                    Id = r.Voucher.Id.Value,
                    Codigo = r.Voucher.Codigo,
                    FechaEmision = r.Voucher.FechaEmision
                },
                Estado = r.Estado.ToString(),
                SaldoPendiente = new MontoDto
                {
                    Valor = r.CalcularSaldoPendiente().Valor,
                    Moneda = r.CalcularSaldoPendiente().Moneda
                }
            }).ToList();
        }
    }
}
