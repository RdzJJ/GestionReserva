using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.DTOs
{
    /// <summary>DTO para un detalle de servicio.</summary>
    public class DetalleServicioDto
    {
        public TipoServicio Tipo { get; set; }
        public string Descripcion { get; set; }
        public MontoDto Precio { get; set; }
        public bool ConfirmadoExternamente { get; set; }
    }
}