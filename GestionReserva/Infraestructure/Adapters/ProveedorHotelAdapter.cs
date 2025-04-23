using Core.Entities;
using Core.Interfaces;


namespace Infraestructure.Adapters
{
    public class ProveedorHotelAdapter : IProveedorAdapter
    {
        public bool ConsultarDisponibilidad(ServicioReservado servicio)
        {
            return true; // para la simulación
            throw new NotImplementedException();
        }
    }
}