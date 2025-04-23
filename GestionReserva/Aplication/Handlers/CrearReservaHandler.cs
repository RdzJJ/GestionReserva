using System;
using Core.Aggregates;
using Core.Entities;
using Core.Interfaces;
using Application.Commands;

namespace Application.Handlers
{
    public class CrearReservaHandler
    {
        private readonly IEnumerable<IProveedorAdapter> _proveedores;
        private readonly IPagoService _pagoService;
        private readonly IReservaRepository _reservaRepository;

        public CrearReservaHandler(IEnumerable<IProveedorAdapter> proveedores, IPagoService pagoService, IReservaRepository reservaRepository)
        {
            _proveedores = proveedores;
            _pagoService = pagoService;
            _reservaRepository = reservaRepository;
        }

        public bool Handle(CrearReservaCommand command)
        {
            // 1. Consultar disponibilidad en todos los servicios de la oferta
            var servicios = new[] { command.Oferta.Vuelo, command.Oferta.Hotel, command.Oferta.Tour };
            foreach (var servicio in servicios)
            {
                if (servicio == null) continue; // Puede que no haya vuelo, hotel o tour

                // Buscar adaptador por tipo
                var tipo = servicio.GetType().Name.Replace("DetalleServicio", "");
                var adapter = _proveedores.FirstOrDefault(p => p.GetType().Name.Contains(tipo));
                if (adapter == null)
                {
                    Console.WriteLine($"No se encontró adaptador para tipo: {tipo}");
                    return false;
                }
                if (!adapter.ConsultarDisponibilidad(servicio))
                {
                    Console.WriteLine($"No hay disponibilidad para tipo: {tipo}");
                    return false;
                }
            }

            // 2. Crear reserva
            var reserva = new Reserva
            {
                Oferta = command.Oferta,
                MontoTotal = command.MontoTotal,
                Estado = Core.Aggregates.EstadoReserva.Pendiente,
                Pagos = new System.Collections.Generic.List<Pago>()
            };

            _reservaRepository.Add(reserva);

            // 3. Procesar pago
            var pagoExitoso = _pagoService.ProcesarPago(reserva.Id, command.MontoTotal, command.PagoCompleto);
            if (!pagoExitoso)
                return false;

            reserva.Estado = Core.Aggregates.EstadoReserva.Confirmada;
            reserva.Pagos.Add(new Pago
            {
                Monto = command.MontoTotal,
                FechaPago = DateTime.Now,
                EsPagoCompleto = command.PagoCompleto
            });
            _reservaRepository.Update(reserva);

            // 4. (Opcional) Generar y asociar voucher
            // reserva.Voucher = new Voucher { ... };

            return true;
        }
    }
}