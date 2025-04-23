using MediatR;
using System;

namespace GestionReserva.Application.Commands
{
    /// <summary>Comando interno para procesar confirmación de pago externo.</summary>
    public class ProcesarConfirmacionPagoCommand : IRequest<bool>
    {
        public Guid ReservaId { get; set; }
        public Guid PagoIdInterno { get; set; }
        public bool Exitoso { get; set; }
    }
}