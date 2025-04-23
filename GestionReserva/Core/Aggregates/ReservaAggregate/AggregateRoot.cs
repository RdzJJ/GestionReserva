using MediatR; // Para INotification si se usa MediatR para eventos
using GestionReserva.Core.Events; // Namespace de DomainEvent
using System.Collections.Generic;
using System.Linq;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Clase base abstracta para todos los Aggregate Roots.
    // Proporciona manejo básico de Eventos de Dominio.
    public abstract class AggregateRoot
    {
        // Lista privada para almacenar los eventos de dominio generados.
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        // Exposición pública de solo lectura de los eventos.
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // Método protegido para que las clases derivadas añadan eventos.
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // Método para limpiar la lista de eventos (usualmente después de despacharlos).
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        // Podría incluirse versionado para concurrencia optimista.
        // public int Version { get; protected set; }
    }
}