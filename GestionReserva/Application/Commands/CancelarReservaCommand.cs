using MediatR;
using System;

namespace GestionReserva.Application.Commands
{
    /// <summary>Comando para solicitar la cancelaci�n de una reserva.</summary>
    public class CancelarReservaCommand : IRequest<bool>
    {
        public Guid ReservaId { get; set; }
    }
}
