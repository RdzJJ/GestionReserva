using Core.Entities;
using Core.Interfaces;
using Core.Aggregates;
using Core.ValueObjects;

namespace Infrastructure.Adapters
{
    public class ProveedorTourAdapter : IProveedorAdapter
    {
        public bool ConsultarDisponibilidad(DetalleServicio servicio)
        {
            return true; // para la simulación
        }
    }
}