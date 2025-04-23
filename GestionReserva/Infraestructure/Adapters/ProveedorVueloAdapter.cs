using Core.Entities;
using Core.Interfaces;


namespace Infraestructure.Adapters
{
    public class ProveedorVueloAdapter : IProveedorAdapter
    {
        public bool ConsultarDisponibilidad(ServicioReservado servicio)
        {
            return true; //por defecto para la simulacion
            throw new NotImplementedException();
        }
    }
}