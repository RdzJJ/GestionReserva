namespace GestionReserva.Core.Events
{
	// Un pago fue confirmado por el sistema externo (webhook).
	public class PagoConfirmadoExternamenteEvent : DomainEvent 
	{ 
		public ValueObjects.ReservaId ReservaId { get; } 
		public ValueObjects.PagoId PagoId { get; } 
		public PagoConfirmadoExternamenteEvent(ValueObjects.ReservaId reservaId, ValueObjects.PagoId pagoId) 
		{ 
			ReservaId = reservaId; PagoId = pagoId; 
		} 
	}

}