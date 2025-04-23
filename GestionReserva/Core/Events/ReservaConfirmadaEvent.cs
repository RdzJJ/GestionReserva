namespace GestionReserva.Core.Events
{
   
    // Una reserva fue confirmada (todos los requisitos cumplidos).
    public class ReservaConfirmadaEvent : DomainEvent 
    { 
        public ValueObjects.ReservaId ReservaId { get; } 
        public ValueObjects.VoucherId VoucherId { get; } 
        public ReservaConfirmadaEvent(ValueObjects.ReservaId reservaId, ValueObjects.VoucherId voucherId) 
        { 
            ReservaId = reservaId; VoucherId = voucherId; 
        } 
    }

}