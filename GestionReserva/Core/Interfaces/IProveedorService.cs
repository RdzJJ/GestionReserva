using GestionReserva.Core.Aggregates.ReservaAggregate;
using System.Threading.Tasks;
namespace GestionReserva.Core.Interfaces
{
    // Contrato para la interacci�n con el sistema externo de Proveedores/Servicios Tur�sticos.
    // Define las operaciones que el Core necesita realizar HACIA ESE sistema.
    public interface IProveedorService
    {
        // Verifica la disponibilidad de todos los servicios en una oferta. (Llamada SALIENTE)
        Task<bool> ConfirmarDisponibilidadServiciosAsync(OfertaPersonalizada oferta);
        // Notifica al proveedor sobre una cancelaci�n. (Llamada SALIENTE)
        Task NotificarCancelacionServiciosAsync(OfertaPersonalizada oferta);
    }
}