using System.Collections.Generic;

namespace GestionReserva.Application.DTOs
{
    /// <summary>DTO para la oferta personalizada de una reserva.</summary>
    public class OfertaDto
    {
        public DestinoDto Destino { get; set; }
        public FechasDeViajeDto Fechas { get; set; }
        public IEnumerable<DetalleServicioDto> DetallesServicio { get; set; }
        public MontoDto CostoTotal { get; set; }
    }
}