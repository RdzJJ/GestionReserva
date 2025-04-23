using Core.Entities;
using Core.Interfaces;
using Core.Aggregates;
using Core.ValueObjects;

namespace Infrastructure.Adapters
{
    public class ProveedorVueloAdapter : IProveedorAdapter
    {
        public bool ConsultarDisponibilidad(DetalleServicio servicio)
        {
            return true; // Simulación
        }
    }
}