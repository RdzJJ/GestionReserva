using MediatR;
using System;
using GestionReserva.Application.DTOs;

namespace GestionReserva.Application.Queries
{
    /// <summary>Consulta para obtener una reserva por su ID.</summary>
    public class ObtenerReservaPorIdQuery : IRequest<ReservaDto>
    {
        public Guid Id { get; set; }
    }
}