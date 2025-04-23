using Core.Entities;
using Core.ValueObjects;

namespace Core.Interfaces
{
    public interface IProveedorAdapter
    {
        bool ConsultarDisponibilidad(DetalleServicio servicio);
    }
}