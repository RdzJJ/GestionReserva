using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestionReserva.Application.Commands; 
using GestionReserva.Application.DTOs;    
using GestionReserva.Core.Interfaces;     
using GestionReserva.Core.ValueObjects; 
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GestionReserva.API.Controllers
{
    // Controlador para manejar Webhooks (callbacks) entrantes desde sistemas de pago externos.
    [ApiController]
    [Route("api/webhooks/pagos")] // Ruta específica para webhooks de pago.
    [ApiExplorerSettings(IgnoreApi = true)] // Oculta este controlador de Swagger UI.
    public class PagosWebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReservaRepository _reservaRepository; // Necesario para mapear ID externo a interno.

        public PagosWebhookController(IMediator mediator, IReservaRepository reservaRepository)
        {
            _mediator = mediator;
            _reservaRepository = reservaRepository; // Inyecta el repositorio.
        }

        // POST /api/webhooks/pagos/confirmacion
        // Recibe la notificación de confirmación de pago.
        [HttpPost("confirmacion")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public async Task<IActionResult> RecibirConfirmacionPago([FromBody] PagoConfirmadoWebhookDto payload)
        {
            
            Console.WriteLine($"[WEBHOOK RECEIVED] Payment Confirmation: PagoID={payload.IdPago}, Estado={payload.Estado}");
            if (payload == null) return BadRequest("Invalid payload.");

            
            
            Guid? reservaId = null; Guid? pagoIdInterno = null;
            // Intenta buscar por ReferenciaReserva si existe y es un Guid válido.
            if (!string.IsNullOrEmpty(payload.ReferenciaReserva) && Guid.TryParse(payload.ReferenciaReserva, out Guid refReservaId))
            {
                var reserva = await _reservaRepository.GetByIdAsync(new ReservaId(refReservaId));
                if (reserva != null)
                {
                    // Busca el primer pago no confirmado.
                    var pago = reserva.Pagos.FirstOrDefault(p => !p.ConfirmadoExternamente);
                    if (pago != null) { reservaId = reserva.Id.Value; pagoIdInterno = pago.Id.Value; }
                }
            }
            if (pagoIdInterno == null || reservaId == null)
            {
                Console.WriteLine($"[WEBHOOK ERROR] Could not map external payment {payload.IdPago} to internal payment/reservation.");
                return Ok(new { status = "MappingError", message = "Could not identify internal payment." }); // OK para no reintentar.
            }
            

            // Crea el comando para procesar la confirmación.
            var command = new ProcesarConfirmacionPagoCommand
            {
                ReservaId = reservaId.Value,
                PagoIdInterno = pagoIdInterno.Value,
                Exitoso = payload.Estado.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase)
            };
            try
            {
                bool result = await _mediator.Send(command); // Envía el comando al handler.
                // Devuelve OK indicando que se procesó (o se intentó procesar).
                return Ok(new { status = result ? "Processed" : "ProcessedWithWarning" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WEBHOOK ERROR] Exception processing payment confirmation: {ex.Message}");
                // Devuelve 500 si hubo un error inesperado durante el procesamiento.
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "ProcessingError", message = ex.Message });
            }
        }
    }

    
}
