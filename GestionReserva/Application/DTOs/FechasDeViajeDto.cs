using System;

namespace GestionReserva.Application.DTOs
{
    /// <summary>DTO para representar fechas de viaje.</summary>
    public class FechasDeViajeDto
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }
}