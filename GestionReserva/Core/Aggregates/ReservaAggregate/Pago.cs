using GestionReserva.Core.ValueObjects;
using System;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Representa un pago realizado asociado a una Reserva.
    // Es una Entidad dentro del Agregado Reserva, tiene identidad propia (PagoId).
    public class Pago
    {
        public PagoId Id { get; private set; } // Identificador �nico del pago (VO).
        public Monto MontoPagado { get; private set; } // Cantidad pagada (VO).
        public TipoPago Tipo { get; private set; } // Tipo de pago (Parcial/Total - Enum).
        public DateTime FechaPago { get; private set; } // Fecha en que se registr� el pago.
        public bool ConfirmadoExternamente { get; private set; } // NUEVO: Estado de confirmaci�n externa.

        // Constructor privado para EF Core.
        private Pago() { }

        // Constructor p�blico: Valida y asigna datos. Inicializa estado de confirmaci�n.
        public Pago(PagoId id, Monto monto, TipoPago tipo)
        {
            // Validaciones (ID no nulo, monto > 0)...
            Id = id; MontoPagado = monto; Tipo = tipo; FechaPago = DateTime.UtcNow;
            ConfirmadoExternamente = false; // Inicia como no confirmado.
        }

        // M�todo interno para actualizar el estado de confirmaci�n.
        internal void MarcarComoConfirmadoExternamente()
        {
            ConfirmadoExternamente = true;
        }
    }
}