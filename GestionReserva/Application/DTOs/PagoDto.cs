using System;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.DTOs
{
    /// <summary>DTO para representar un pago.</summary>
    public class PagoDto
    {
        public Guid Id { get; set; }
        public MontoDto MontoPagado { get; set; }
        public TipoPago Tipo { get; set; }
        public DateTime FechaPago { get; set; }
        public bool ConfirmadoExternamente { get; set; }
    }
}