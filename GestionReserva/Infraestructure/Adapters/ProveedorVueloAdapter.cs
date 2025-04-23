using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Adapters
{
    public class ProveedorVueloAdapter : IProveedorAdapter
    {
        public bool ConsultarDisponibilidad(ServicioReservado servicio)
        {
            return true; // Simulación
        }
    }
}