using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    /// <summary>
    /// Identificador único tipado para un Pago.
    /// Envuelve un Guid con validaciones y lógica de igualdad.
    /// </summary>
    public class PagoId : ValueObject
    {
        public Guid Value { get; }

        public PagoId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Pago ID cannot be empty.", nameof(value));
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(PagoId id) => id.Value;
        public static implicit operator PagoId(Guid value) => new PagoId(value);
    }
}
