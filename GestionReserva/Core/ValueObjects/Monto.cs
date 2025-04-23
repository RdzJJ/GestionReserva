using System;
using System.Collections.Generic;

namespace GestionReserva.Core.ValueObjects
{
    // Representa una cantidad monetaria con valor y moneda.
    public class Monto : ValueObject
    {
        public decimal Valor { get; } // La cantidad numérica.
        public string Moneda { get; } // El código de la moneda (ej: "COP", "USD").

        // Constructor: Valida que el valor no sea negativo y la moneda no esté vacía.
        public Monto(decimal valor, string moneda)
        {
            if (valor < 0) throw new ArgumentException("Amount value cannot be negative.", nameof(valor));
            if (string.IsNullOrWhiteSpace(moneda)) throw new ArgumentException("Currency cannot be empty.", nameof(moneda));
            Valor = valor; Moneda = moneda;
        }

        // Componentes de igualdad: Valor y Moneda.
        protected override IEnumerable<object> GetEqualityComponents() { yield return Valor; yield return Moneda; }
    }
}