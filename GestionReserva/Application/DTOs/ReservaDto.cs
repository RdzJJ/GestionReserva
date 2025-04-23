using System;
using System.Collections.Generic;

namespace GestionReserva.Application.DTOs
{
    /// <summary>DTO para representar una reserva completa.</summary>
    public class ReservaDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public OfertaDto Oferta { get; set; }
        public IEnumerable<PagoDto> Pagos { get; set; }
        public VoucherDto Voucher { get; set; }
        public string Estado { get; set; }
        public MontoDto SaldoPendiente { get; set; }
    }
}
