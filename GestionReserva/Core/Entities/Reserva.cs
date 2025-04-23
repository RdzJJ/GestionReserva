using System.Collections.Generic;

namespace Core.Entities
{
    public class Reserva
    {
        public int Id { get; private set; }
        public List<ServicioReservado> Servicios { get; private set; }
        public EstadoReserva Estado { get; private set; }
        public decimal MontoTotal { get; private set; }

        public Reserva(List<ServicioReservado> servicios, decimal montoTotal)
        {
            Servicios = servicios;
            MontoTotal = montoTotal;
            Estado = EstadoReserva.PendienteConfirmacion;
        }

        public void Confirmar() => Estado = EstadoReserva.Confirmada;
        public void MarcarPagada() => Estado = EstadoReserva.Pagada;
    }

    public enum EstadoReserva
    {
        PendienteConfirmacion,
        Confirmada,
        Pagada
    }

    public class ServicioReservado
    {
        public string Tipo { get; set; } // "Vuelo", "Hotel"
        public string IdExterno { get; set; }
    }
}