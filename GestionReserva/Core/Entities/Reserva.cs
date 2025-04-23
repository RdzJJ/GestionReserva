using System.Collections.Generic;

namespace Core.Entities
{
    public class Reserva
    {
        public int Id { get; set; }
        public List<ServicioReservado> Servicios { get; set; } = new();
        public EstadoReserva Estado { get; set; }
        public decimal MontoTotal { get; set; }

        // Constructor sin parametros
        public Reserva() { }

        // Constructor Original
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
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string IdExterno { get; set; } = string.Empty;
        public int ReservaId { get; set; }
        public Reserva? Reserva { get; set; }

        public ServicioReservado() { }
    }
}