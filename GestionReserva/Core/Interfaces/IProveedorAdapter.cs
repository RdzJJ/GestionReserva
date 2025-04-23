using Core.Entities;

namespace Core.Interfaces
{
    public interface IProveedorAdapter
    {
        bool ConsultarDisponibilidad(ServicioReservado servicio);
    }
}