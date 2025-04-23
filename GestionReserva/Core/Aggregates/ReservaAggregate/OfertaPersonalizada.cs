using GestionReserva.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Representa la oferta seleccionada por el usuario, contenida dentro del Agregado Reserva.
    // Podría ser una Entidad si tuviera ciclo de vida propio, pero aquí es parte de Reserva.
    public class OfertaPersonalizada // No hereda de ValueObject, podría tener identidad si fuera entidad separada.
    {
        public Destino Destino { get; private set; } // Destino de la oferta (VO).
        public FechasDeViaje Fechas { get; private set; } // Fechas de la oferta (VO).
        private readonly List<DetalleServicio> _detallesServicio = new List<DetalleServicio>(); // Lista interna de servicios.
        public IReadOnlyCollection<DetalleServicio> DetallesServicio => _detallesServicio.AsReadOnly(); // Expone como solo lectura.

        // Constructor privado para EF Core.
        private OfertaPersonalizada() { }

        // Constructor público: Valida y asigna los datos. Clona los detalles.
        public OfertaPersonalizada(Destino destino, FechasDeViaje fechas, IEnumerable<DetalleServicio> detalles)
        {
            // Validaciones...
            Destino = destino; Fechas = fechas;
            // Se crean nuevas instancias de DetalleServicio para asegurar que pertenezcan a esta oferta.
            _detallesServicio.AddRange(detalles.Select(d => new DetalleServicio(d.Tipo, d.Descripcion, d.Precio)));
        }

        // Calcula el costo total sumando los precios de los detalles.
        public Monto CalcularCostoTotal()
        {
            // Lógica para sumar precios, manejando monedas...
            if (!_detallesServicio.Any()) return new Monto(0, "COP"); // Moneda por defecto si no hay servicios
            string moneda = _detallesServicio.First().Precio.Moneda;
            if (_detallesServicio.Any(d => d.Precio.Moneda != moneda)) throw new InvalidOperationException("Cannot calculate total cost with mixed currencies.");
            decimal total = _detallesServicio.Sum(d => d.Precio.Valor);
            return new Monto(total, moneda);
        }

        // Marca un servicio específico como confirmado externamente. Devuelve true si hubo cambio.
        internal bool MarcarServicioComoConfirmado(TipoServicio tipo, string descripcion)
        {
            var servicio = _detallesServicio.FirstOrDefault(d => d.Tipo == tipo && d.Descripcion == descripcion);
            if (servicio != null && !servicio.ConfirmadoExternamente)
            {
                servicio.MarcarComoConfirmadoExternamente(); // Llama al método interno de DetalleServicio
                return true;
            }
            return false;
        }

        // Verifica si todos los servicios de la oferta han sido confirmados externamente.
        internal bool TodosLosServiciosConfirmadosExternamente()
        {
            return _detallesServicio.All(d => d.ConfirmadoExternamente);
        }
    }
}