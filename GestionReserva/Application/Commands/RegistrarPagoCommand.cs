using MediatR;
using System;
using GestionReserva.Application.DTOs;

namespace GestionReserva.Application.Commands
{
    /// <summary>Comando para registrar un pago adicional.</summary>
    public class RegistrarPagoCommand : IRequest<Guid>
    {
        public Guid ReservaId { get; set; }
        public PagoDto NuevoPago { get; set; }
    }
}