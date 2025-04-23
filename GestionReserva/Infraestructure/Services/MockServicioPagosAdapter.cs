using GestionReserva.Core.Interfaces;
using GestionReserva.Core.ValueObjects;
using System;
using System.Threading.Tasks;
namespace GestionReserva.Infrastructure.Services
{
    // Implementación MOCK (simulada) de IServicioPagos.
    public class MockServicioPagosAdapter : IServicioPagos
    {
        // Simula la confirmación de pago (siempre devuelve true).
        public async Task<bool> ConfirmarPagoRecibidoAsync(PagoId pagoId)
        {
            Console.WriteLine($"[MOCK OUTBOUND] Confirming payment received for PagoId: {pagoId.Value}");
            await Task.Delay(40);
            Console.WriteLine($"[MOCK OUTBOUND] Payment confirmed for PagoId: {pagoId.Value}.");
            return true;
        }
        // Simula el procesamiento de reembolso (siempre devuelve true).
        public async Task<bool> ProcesarReembolsoAsync(PagoId pagoId, Monto monto)
        {
            Console.WriteLine($"[MOCK OUTBOUND] Processing refund for PagoId: {pagoId.Value}, Amount: {monto.Valor} {monto.Moneda}");
            await Task.Delay(60);
            Console.WriteLine($"[MOCK OUTBOUND] Refund processed for PagoId: {pagoId.Value}.");
            return true;
        }
    }
}