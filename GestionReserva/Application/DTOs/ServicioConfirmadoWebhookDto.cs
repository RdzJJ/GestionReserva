using GestionReserva.Core.ValueObjects;
using System;

namespace GestionReserva.Application.DTOs
{
    /// <summary>Payload de webhook de confirmación de servicio.</summary>
    public class ServicioConfirmadoWebhookDto
    {
        public Guid ReservaId { get; set; }
        public TipoServicio TipoServicio { get; set; }
        public string DescripcionServicio { get; set; }
        public string Estado { get; set; }
    }
}
