using MediatR;
using GestionReserva.Core.ValueObjects;
using System;

namespace GestionReserva.Application.Commands
{
    /// <summary>Comando interno para procesar confirmación de servicio externo.</summary>
    public class ProcesarConfirmacionProveedorCommand : IRequest<bool>
    {
        public Guid ReservaId { get; set; }
        public TipoServicio TipoServicio { get; set; }
        public string DescripcionServicio { get; set; }
        public bool Exitoso { get; set; }
    }
}