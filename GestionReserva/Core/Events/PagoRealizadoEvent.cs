namespace GestionReserva.Core.Events
{
    // Se registró un pago (aún no necesariamente confirmado externamente).
    public class PagoRealizadoEvent : DomainEvent 
    { 
        public ValueObjects.ReservaId ReservaId { get; } 
        public ValueObjects.PagoId PagoId { get; } 
        public ValueObjects.Monto MontoPagado { get; } 
        public PagoRealizadoEvent(ValueObjects.ReservaId reservaId, ValueObjects.PagoId pagoId, ValueObjects.Monto montoPagado) 
        { 
            ReservaId = reservaId; PagoId = pagoId; MontoPagado = montoPagado; 
        } 
    }

}