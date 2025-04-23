using GestionReserva.Core.ValueObjects;
using System;
using System.Collections.Generic; // Necesario para GetEqualityComponents

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Representa un ítem específico dentro de una Oferta (Vuelo, Hotel, Tour).
    // Modelado como Value Object dentro de OfertaPersonalizada.
    public class DetalleServicio : ValueObject
    {
        public TipoServicio Tipo { get; }      // Tipo de servicio (Enum).
        public string Descripcion { get; }     // Descripción textual.
        public Monto Precio { get; }           // Precio del servicio (usando Monto VO).
        public bool ConfirmadoExternamente { get; private set; } // NUEVO: Estado de confirmación externa.

        // Constructor privado para EF Core.
        private DetalleServicio() { }

        // Constructor público: Inicializa propiedades y estado de confirmación.
        public DetalleServicio(TipoServicio tipo, string descripcion, Monto precio)
        {
            if (string.IsNullOrWhiteSpace(descripcion)) throw new ArgumentException("Service description cannot be empty.", nameof(descripcion));
            if (precio == null) throw new ArgumentNullException(nameof(precio));
            Tipo = tipo; Descripcion = descripcion; Precio = precio;
            ConfirmadoExternamente = false; // Inicia como no confirmado.
        }

        // Componentes de igualdad: Tipo, Descripción, Precio. La confirmación no define identidad.
        protected override IEnumerable<object> GetEqualityComponents() { yield return Tipo; yield return Descripcion; yield return Precio; }

        // Método interno para ser llamado por la Oferta/Reserva para actualizar el estado.
        internal void MarcarComoConfirmadoExternamente() { ConfirmadoExternamente = true; }
        internal void MarcarComoNoConfirmadoExternamente() { ConfirmadoExternamente = false; }
    }
}