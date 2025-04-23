namespace GestionReserva.Application.DTOs
{
    /// <summary>Payload de webhook de confirmación de pago.</summary>
    public class PagoConfirmadoWebhookDto
    {
        public string IdPago { get; set; }
        public string ReferenciaReserva { get; set; }
        public string Estado { get; set; }
    }
}