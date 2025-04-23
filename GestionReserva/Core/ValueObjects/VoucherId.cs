using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    /// <summary>
    /// Identificador único tipado para un Voucher.
    /// Envuelve un Guid con validaciones y lógica de igualdad.
    /// </summary>
    public class VoucherId : ValueObject
    {
        public Guid Value { get; }

        public VoucherId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Voucher ID cannot be empty.", nameof(value));
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(VoucherId id) => id.Value;
        public static implicit operator VoucherId(Guid value) => new VoucherId(value);
    }
}