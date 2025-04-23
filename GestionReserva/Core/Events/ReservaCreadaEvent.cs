namespace GestionReserva.Core.Events
{
    // Se creó una nueva reserva.
    public class ReservaCreadaEvent : DomainEvent 
    { 
        public ValueObjects.ReservaId ReservaId { get; } 
        public ReservaCreadaEvent(ValueObjects.ReservaId reservaId) 
        { 
            ReservaId = reservaId; 
        } 
    }

}