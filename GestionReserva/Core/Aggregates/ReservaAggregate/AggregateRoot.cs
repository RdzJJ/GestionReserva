using MediatR; // Para INotification si se usa MediatR para eventos
using GestionReserva.Core.Events; // Namespace de DomainEvent
using System.Collections.Generic;
using System.Linq;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // Clase base abstracta para todos los Aggregate Roots.
    // Proporciona manejo b�sico de Eventos de Dominio.
    public abstract class AggregateRoot
    {
        // Lista privada para almacenar los eventos de dominio generados.
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        // Exposici�n p�blica de solo lectura de los eventos.
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // M�todo protegido para que las clases derivadas a�adan eventos.
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // M�todo para limpiar la lista de eventos (usualmente despu�s de despacharlos).
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        // Podr�a incluirse versionado para concurrencia optimista.
        // public int Version { get; protected set; }
    }
}