namespace GestionReserva.Core.Events
{
    // Todos los servicios de una reserva fueron confirmados externamente.
    public class TodosServiciosConfirmadosExternamenteEvent : DomainEvent 
    { 
        public ValueObjects.ReservaId ReservaId { get; } 
        public TodosServiciosConfirmadosExternamenteEvent(ValueObjects.ReservaId reservaId) 
        { 
            ReservaId = reservaId; 
        } 
    }

}