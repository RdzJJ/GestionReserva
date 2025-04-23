using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Application.Commands;
using Application.Handlers;
using Infrastructure.Adapters;
using Infrastructure.Services;
using Infrastructure.Repositories;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly CrearReservaHandler _handler;

        public ReservaController()
        {
            var proveedores = new List<IProveedorAdapter>
            {
                new ProveedorHotelAdapter(),
                new ProveedorVueloAdapter(),
            };
            var pagoService = new PagoService();
            var reservaRepository = new ReservaRepository();
            _handler = new CrearReservaHandler(proveedores, pagoService, reservaRepository);
        }

        [HttpPost]
        public IActionResult CrearReserva(CrearReservaCommand command)
        {
            var resultado = _handler.Handle(command);
            if (!resultado)
                return BadRequest("No se pudo crear la reserva.");
            return Ok("Reserva creada y pagada exitosamente.");
        }
    }
}