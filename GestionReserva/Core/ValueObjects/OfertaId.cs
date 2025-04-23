using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    /// <summary>
    /// Identificador único tipado para una Oferta.
    /// Envuelve un Guid con validaciones y lógica de igualdad.
    /// </summary>
    public class OfertaId : ValueObject
    {
        public Guid Value { get; }

        public OfertaId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Oferta ID cannot be empty.", nameof(value));
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(OfertaId id) => id.Value;
        public static implicit operator OfertaId(Guid value) => new OfertaId(value);
    }
}