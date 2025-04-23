using GestionReserva.Core.ValueObjects;
using System.Threading.Tasks;
namespace GestionReserva.Core.Interfaces
{
    // Contrato para la interacción con el sistema externo de Pagos.
    // Define las operaciones que el Core necesita realizar HACIA ESE sistema.
    public interface IServicioPagos
    {
        // Procesa un reembolso para un pago específico. (Llamada SALIENTE)
        Task<bool> ProcesarReembolsoAsync(PagoId pagoId, Monto monto);
        // Confirma si un pago fue recibido (si la confirmación es asíncrona). (Llamada SALIENTE, quizás no necesaria con webhooks)
        Task<bool> ConfirmarPagoRecibidoAsync(PagoId pagoId);
    }
}