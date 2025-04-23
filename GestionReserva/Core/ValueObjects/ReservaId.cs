using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    // Representa el identificador �nico de una Reserva como un Value Object.
    // Envuelve un Guid para darle significado de dominio y asegurar validaciones.
    public class ReservaId : ValueObject
    {
        public Guid Value { get; } // El valor Guid subyacente.

        // Constructor: Valida que el Guid no sea vac�o.
        public ReservaId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Reserva ID cannot be empty.", nameof(value));
            Value = value;
        }

        // Define el componente de igualdad (el propio Guid).
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        // Conversiones impl�citas para facilitar el uso con Guids est�ndar.
        public static implicit operator Guid(ReservaId reservaId) => reservaId.Value;
        public static implicit operator ReservaId(Guid value) => new ReservaId(value);
    }
    // Implementaciones similares para OfertaId, PagoId, VoucherId...
}