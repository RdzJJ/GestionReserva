using GestionReserva.Core.ValueObjects;
using System;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Representa el comprobante digital emitido cuando una Reserva se confirma.
    // Es una Entidad (o VO si es simple) dentro del Agregado Reserva.
    public class Voucher
    {
        public VoucherId Id { get; private set; } // Identificador único del voucher (VO).
        public string Codigo { get; private set; } // El código alfanumérico del voucher.
        public DateTime FechaEmision { get; private set; } // Fecha de emisión.

        // Constructor privado para EF Core.
        private Voucher() { }

        // Constructor público: Valida y asigna datos.
        public Voucher(VoucherId id, string codigo)
        {
            // Validaciones (ID no nulo, código no vacío)...
            Id = id; Codigo = codigo; FechaEmision = DateTime.UtcNow;
        }
    }
}