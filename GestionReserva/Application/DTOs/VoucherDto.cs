using System;

namespace GestionReserva.Application.DTOs
{
    /// <summary>DTO para representar un voucher de reserva.</summary>
    public class VoucherDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaEmision { get; set; }
    }
}