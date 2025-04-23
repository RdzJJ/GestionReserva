using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestionReserva.Application.Commands;
using GestionReserva.Application.DTOs;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;
using System.Linq;

namespace GestionReserva.API.Controllers
{
    [ApiController]
    [Route("api/webhooks/proveedores")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProveedoresWebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReservaRepository _reservaRepository;

        public ProveedoresWebhookController(IMediator mediator, IReservaRepository reservaRepository)
        {
            _mediator = mediator;
            _reservaRepository = reservaRepository;
        }

        [HttpPost("confirmacion-servicio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecibirConfirmacionServicio([FromBody] ServicioConfirmadoWebhookDto payload)
        {
            if (payload == null) return BadRequest("Invalid payload.");

            // Ejemplo de mapeo simplificado: buscar la reserva y el detalle de servicio.
            ReservaId? reservaId = null;
            var reserva = await _reservaRepository.GetByIdAsync(new ReservaId(payload.ReservaId));
            if (reserva != null)
            {
                reservaId = reserva.Id;
            }
            if (reservaId == null)
                return Ok(new { status = "MappingError", message = "Could not identify reservation." });

            var command = new ProcesarConfirmacionProveedorCommand
            {
                ReservaId = reservaId.Value,
                TipoServicio = payload.TipoServicio,
                DescripcionServicio = payload.DescripcionServicio,
                Exitoso = payload.Estado.Equals("CONFIRMED", StringComparison.OrdinalIgnoreCase)
            };

            try
            {
                bool result = await _mediator.Send(command);
                return Ok(new { status = result ? "Processed" : "ProcessedWithWarning" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "ProcessingError", message = ex.Message });
            }
        }
    }
}