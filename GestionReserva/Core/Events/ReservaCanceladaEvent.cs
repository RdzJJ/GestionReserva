namespace GestionReserva.Core.Events
{
    // Una reserva fue cancelada.
    public class ReservaCanceladaEvent : DomainEvent 
    { 
        public ValueObjects.ReservaId ReservaId { get; } 
        public ReservaCanceladaEvent(ValueObjects.ReservaId reservaId) 
        { 
            ReservaId = reservaId; 
        } 
    }
}