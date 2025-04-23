using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    /// <summary>
    /// Representa las fechas de inicio y fin de un viaje.
    /// </summary>
    public class FechasDeViaje : ValueObject
    {
        public DateTime Inicio { get; }
        public DateTime Fin { get; }

        public FechasDeViaje(DateTime inicio, DateTime fin)
        {
            if (inicio > fin)
                throw new ArgumentException("Start date must be before end date.", nameof(inicio));
            Inicio = inicio;
            Fin = fin;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Inicio;
            yield return Fin;
        }
    }
}
