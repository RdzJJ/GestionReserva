using Core.ValueObjects;
using Core.Entities;
using System.Collections.Generic;

namespace Core.Aggregates
{
    public class Reserva
    {
        public int Id { get; set; }
        public OfertaPersonalizada Oferta { get; set; } = new();
        public List<Pago> Pagos { get; set; } = new();
        public Voucher? Voucher { get; set; }
        public EstadoReserva Estado { get; set; }
        public decimal MontoTotal { get; set; }
    }

    public enum EstadoReserva
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Modificada
    }
}