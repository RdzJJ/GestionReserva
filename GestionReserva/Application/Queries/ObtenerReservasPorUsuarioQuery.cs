using System;
using System.Collections.Generic;
using MediatR;
using GestionReserva.Application.DTOs;

namespace GestionReserva.Application.Queries
{
    
    public class ObtenerReservasPorUsuarioQuery : IRequest<List<ReservaDto>>
    {
        public Guid UsuarioId { get; set; }
    }
}