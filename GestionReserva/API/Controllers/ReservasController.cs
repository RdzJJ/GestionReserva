using MediatR;
using Microsoft.AspNetCore.Mvc;
using GestionReserva.Application.Commands;
using GestionReserva.Application.Queries;
using GestionReserva.Application.DTOs;
using System;
using System.Threading.Tasks;
using System.Collections.Generic; // Para List<ReservaDto>
using Microsoft.AspNetCore.Http; // Para StatusCodes

namespace GestionReserva.API.Controllers
{
    // Controlador principal para gestionar las operaciones sobre las Reservas.
    [ApiController] // Indica que es un controlador de API.
    [Route("api/[controller]")] // Define la ruta base: "api/reservas".
    [Produces("application/json")] // Indica que produce respuestas JSON.
    public class ReservasController : ControllerBase // Hereda de la clase base para controladores API.
    {
        private readonly IMediator _mediator; // Inyecta MediatR para enviar Commands y Queries.

        // Constructor para inyección de dependencias.
        public ReservasController(IMediator mediator) { _mediator = mediator; }


        // POST /api/reservas
        // Crea una nueva reserva.
        [HttpPost]
        [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var reservaId = await _mediator.Send(command);
                var query = new ObtenerReservaPorIdQuery { Id = reservaId };
                var reservaDto = await _mediator.Send(query);
                if (reservaDto == null)
                    return Problem("Failed to retrieve reservation after creation.", statusCode: StatusCodes.Status500InternalServerError);

                return CreatedAtAction(nameof(ObtenerReservaPorId), new { id = reservaId }, reservaDto);
            }
            catch (Exception ex)
            {
                // Si existe InnerException, mostrarla (suele contener la causa real).
                var error = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = $"Error creating reservation: {error}" });
            }
        }

        // GET /api/reservas/{id}
        // Obtiene una reserva por su ID.
        [HttpGet("{id:guid}")] // Define ruta con parámetro 'id' de tipo GUID.
        [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status200OK)] // Documenta respuesta exitosa (200).
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Documenta caso no encontrado (404).
        public async Task<IActionResult> ObtenerReservaPorId(Guid id)
        {
            var query = new ObtenerReservaPorIdQuery { Id = id }; // Crea la Query.
            var reservaDto = await _mediator.Send(query); // Envía la Query al Handler.
            // Devuelve 200 OK con el DTO si se encontró, o 404 Not Found si no.
            return reservaDto != null ? Ok(reservaDto) : NotFound(new { message = $"Reservation with ID {id} not found." });
        }

        // POST /api/reservas/{id}/cancelar
        // Cancela una reserva existente.
        [HttpPost("{id:guid}/cancelar")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            var command = new CancelarReservaCommand { ReservaId = id };
            try
            {
                var success = await _mediator.Send(command);
                if (success)
                    return NoContent();
                else
                    return BadRequest(new { message = "La reserva ya está cancelada o no puede cancelarse." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = $"Cancellation failed: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return Problem($"Error cancelling {id}.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // POST /api/reservas/{id}/pagos
        // Registra un nuevo pago para una reserva.
        [HttpPost("{id:guid}/pagos")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)] // Devuelve el ID del pago creado (201).
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Error de validación/negocio (400).
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Reserva no encontrada (404).
        public async Task<IActionResult> RegistrarPago(Guid id, [FromBody] PagoDto pagoDto)
        {
            if (!ModelState.IsValid || pagoDto.MontoPagado == null || pagoDto.MontoPagado.Valor <= 0) return BadRequest(new { message = "Invalid payment data." });
            var command = new RegistrarPagoCommand { ReservaId = id, NuevoPago = pagoDto }; // Crea Comando.
            try
            {
                var pagoId = await _mediator.Send(command); // Envía Comando.
                // Devuelve 201 Created con la ubicación de la reserva actualizada y el ID del nuevo pago.
                return CreatedAtAction(nameof(ObtenerReservaPorId), new { id = id }, new { pagoId = pagoId });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); } // Manejo no encontrado.
            catch (InvalidOperationException ex) { return BadRequest(new { message = $"Payment registration failed: {ex.Message}" }); } // Manejo reglas de negocio.
            catch (Exception ex) { return Problem($"Error registering payment for {id}.", statusCode: StatusCodes.Status500InternalServerError); } // Manejo genérico.
        }

        // GET /api/reservas/usuario/{usuarioId}
        // Obtiene todas las reservas de un usuario (Implementación de Handler pendiente).
        [HttpGet("usuario/{usuarioId:guid}")]
        [ProducesResponseType(typeof(List<ReservaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerReservasPorUsuario(Guid usuarioId)
        {
            Console.WriteLine("Warning: ObtenerReservasPorUsuario requires repository implementation.");
            var query = new ObtenerReservasPorUsuarioQuery { UsuarioId = usuarioId };
            var reservas = await _mediator.Send(query); // Llama al handler (actualmente devuelve lista vacía).
            return Ok(reservas);
        }
    }
}