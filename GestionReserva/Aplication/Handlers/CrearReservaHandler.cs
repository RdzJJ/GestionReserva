using System.Collections.Generic;
using System.Linq;
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
            // 1. Consultar disponibilidad en todos los proveedores
            foreach (var servicio in command.Servicios)
            {
                var adapter = _proveedores.FirstOrDefault(p => p.GetType().Name.Contains(servicio.Tipo));
                if (adapter == null || !adapter.ConsultarDisponibilidad(servicio))
                    return false; // No disponible
            }

            // 2. Crear reserva
            var reserva = new Reserva(command.Servicios, command.MontoTotal);
            reserva.Confirmar();
            _reservaRepository.Add(reserva);

            // 3. Procesar pago
            var pagoExitoso = _pagoService.ProcesarPago(reserva.Id, command.MontoTotal, command.PagoCompleto);
            if (!pagoExitoso)
                return false;

            reserva.MarcarPagada();
            _reservaRepository.Update(reserva);

            // 4. Enviar vouchers (puedes usar un servicio de notificación aquí)
            // NotificacionService.EnviarVouchers(reserva);

            return true;
        }
    }
}