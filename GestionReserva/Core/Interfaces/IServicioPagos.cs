using GestionReserva.Core.ValueObjects;
using System.Threading.Tasks;
namespace GestionReserva.Core.Interfaces
{
    // Contrato para la interacci�n con el sistema externo de Pagos.
    // Define las operaciones que el Core necesita realizar HACIA ESE sistema.
    public interface IServicioPagos
    {
        // Procesa un reembolso para un pago espec�fico. (Llamada SALIENTE)
        Task<bool> ProcesarReembolsoAsync(PagoId pagoId, Monto monto);
        // Confirma si un pago fue recibido (si la confirmaci�n es as�ncrona). (Llamada SALIENTE, quiz�s no necesaria con webhooks)
        Task<bool> ConfirmarPagoRecibidoAsync(PagoId pagoId);
    }
}