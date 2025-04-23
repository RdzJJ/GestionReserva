using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    /// <summary>
    /// Representa un destino turístico con nombre y país.
    /// </summary>
    public class Destino : ValueObject
    {
        public string Nombre { get; }
        public string Pais { get; }

        public Destino(string nombre, string pais)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Destination name cannot be empty.", nameof(nombre));
            if (string.IsNullOrWhiteSpace(pais))
                throw new ArgumentException("Country cannot be empty.", nameof(pais));
            Nombre = nombre;
            Pais = pais;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Nombre;
            yield return Pais;
        }
    }
}
