using System.Collections.Generic;
using System.Linq;

namespace GestionReserva.Core.ValueObjects
{
    // Clase base abstracta para implementar el patrón Value Object.
    // Proporciona una implementación estándar para la igualdad basada en los componentes.
    public abstract class ValueObject
    {
        // Compara si dos Value Objects son iguales. Maneja casos nulos.
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) { return false; }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        // Compara si dos Value Objects son diferentes.
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        // Método abstracto que deben implementar las clases derivadas.
        // Devuelve los componentes que definen la identidad/igualdad del Value Object.
        protected abstract IEnumerable<object> GetEqualityComponents();

        // Sobrescribe el método Equals para comparar usando los componentes de igualdad.
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType()) { return false; }
            var other = (ValueObject)obj;
            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        // Sobrescribe GetHashCode para que sea consistente con Equals.
        // El hash se basa en los hash de los componentes de igualdad.
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        // Sobrecarga de operadores de igualdad para una comparación más natural.
        public static bool operator ==(ValueObject one, ValueObject two) { return EqualOperator(one, two); }
        public static bool operator !=(ValueObject one, ValueObject two) { return NotEqualOperator(one, two); }
    }
}