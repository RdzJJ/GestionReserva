using GestionReserva.Core.ValueObjects;
using System;
using System.Collections.Generic; // Necesario para GetEqualityComponents

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Representa un �tem espec�fico dentro de una Oferta (Vuelo, Hotel, Tour).
    // Modelado como Value Object dentro de OfertaPersonalizada.
    public class DetalleServicio : ValueObject
    {
        public TipoServicio Tipo { get; }      // Tipo de servicio (Enum).
        public string Descripcion { get; }     // Descripci�n textual.
        public Monto Precio { get; }           // Precio del servicio (usando Monto VO).
        public bool ConfirmadoExternamente { get; private set; } // NUEVO: Estado de confirmaci�n externa.

        // Constructor privado para EF Core.
        private DetalleServicio() { }

        // Constructor p�blico: Inicializa propiedades y estado de confirmaci�n.
        public DetalleServicio(TipoServicio tipo, string descripcion, Monto precio)
        {
            if (string.IsNullOrWhiteSpace(descripcion)) throw new ArgumentException("Service description cannot be empty.", nameof(descripcion));
            if (precio == null) throw new ArgumentNullException(nameof(precio));
            Tipo = tipo; Descripcion = descripcion; Precio = precio;
            ConfirmadoExternamente = false; // Inicia como no confirmado.
        }

        // Componentes de igualdad: Tipo, Descripci�n, Precio. La confirmaci�n no define identidad.
        protected override IEnumerable<object> GetEqualityComponents() { yield return Tipo; yield return Descripcion; yield return Precio; }

        // M�todo interno para ser llamado por la Oferta/Reserva para actualizar el estado.
        internal void MarcarComoConfirmadoExternamente() { ConfirmadoExternamente = true; }
        internal void MarcarComoNoConfirmadoExternamente() { ConfirmadoExternamente = false; }
    }
}