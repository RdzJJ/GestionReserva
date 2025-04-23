using MediatR;
using System;
using System.Collections.Generic;
using GestionReserva.Application.DTOs;
using GestionReserva.Core.ValueObjects;

namespace GestionReserva.Application.Commands
{
    /// <summary>Comando para crear una nueva reserva.</summary>
    public class CrearReservaCommand : IRequest<Guid>
    {
        public Guid UsuarioId { get; set; }
        public DestinoDto Destino { get; set; }
        public FechasDeViajeDto Fechas { get; set; }
        public IEnumerable<DetalleServicioDto> DetallesServicio { get; set; }
        public MontoDto MontoInicial { get; set; }
        public TipoPago TipoPago { get; set; }
    }
}