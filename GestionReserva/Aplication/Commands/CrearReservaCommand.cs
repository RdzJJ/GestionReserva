using System.Collections.Generic;
using Core.Entities;

namespace Application.Commands
{
    public class CrearReservaCommand
    {
        public List<ServicioReservado> Servicios { get; set; }
        public decimal MontoTotal { get; set; }
        public bool PagoCompleto { get; set; }
    }
}