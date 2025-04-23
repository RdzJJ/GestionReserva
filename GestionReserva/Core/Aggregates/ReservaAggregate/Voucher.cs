using GestionReserva.Core.ValueObjects;
using System;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Representa el comprobante digital emitido cuando una Reserva se confirma.
    // Es una Entidad (o VO si es simple) dentro del Agregado Reserva.
    public class Voucher
    {
        public VoucherId Id { get; private set; } // Identificador �nico del voucher (VO).
        public string Codigo { get; private set; } // El c�digo alfanum�rico del voucher.
        public DateTime FechaEmision { get; private set; } // Fecha de emisi�n.

        // Constructor privado para EF Core.
        private Voucher() { }

        // Constructor p�blico: Valida y asigna datos.
        public Voucher(VoucherId id, string codigo)
        {
            // Validaciones (ID no nulo, c�digo no vac�o)...
            Id = id; Codigo = codigo; FechaEmision = DateTime.UtcNow;
        }
    }
}