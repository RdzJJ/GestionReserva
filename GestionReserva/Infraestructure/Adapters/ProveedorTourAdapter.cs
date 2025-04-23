using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Adapters
{
    public class ProveedorTourAdapter : IProveedorAdapter
    {
        public bool ConsultarDisponibilidad(ServicioReservado servicio)
        {
            return true; // para la simulación
        }
    }
}