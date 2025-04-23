namespace GestionReserva.Core.Events
{
	// Un servicio específico fue confirmado por un proveedor externo (webhook).
	public class ServicioConfirmadoExternamenteEvent : DomainEvent 
	{ 
		public ValueObjects.ReservaId ReservaId { get; } 
		public ValueObjects.TipoServicio TipoServicio { get; } 
		public string DescripcionServicio { get; } 
		public ServicioConfirmadoExternamenteEvent(ValueObjects.ReservaId reservaId, ValueObjects.TipoServicio tipo, string desc) 
		{ 
			ReservaId = reservaId; 
			TipoServicio = tipo; 
			DescripcionServicio = desc; 
		} 
	}

}