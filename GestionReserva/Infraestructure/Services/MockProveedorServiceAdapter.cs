using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.Interfaces;
using System;
using System.Threading.Tasks;
namespace GestionReserva.Infrastructure.Services
{
    // Implementaci�n MOCK (simulada) de IProveedorService.
    // No realiza llamadas reales, solo simula respuestas exitosas.
    public class MockProveedorServiceAdapter : IProveedorService
    {
        // Simula la confirmaci�n de disponibilidad (siempre devuelve true).
        public async Task<bool> ConfirmarDisponibilidadServiciosAsync(OfertaPersonalizada oferta)
        {
            Console.WriteLine($"[MOCK OUTBOUND] Checking availability for offer: {oferta.Destino.Nombre}");
            await Task.Delay(50); // Simula latencia de red.
            Console.WriteLine($"[MOCK OUTBOUND] Availability confirmed for offer {oferta.Destino.Nombre}.");
            return true;
        }
        // Simula la notificaci�n de cancelaci�n.
        public async Task NotificarCancelacionServiciosAsync(OfertaPersonalizada oferta)
        {
            Console.WriteLine($"[MOCK OUTBOUND] Notifying providers about cancellation for offer: {oferta.Destino.Nombre}");
            await Task.Delay(30);
            Console.WriteLine($"[MOCK OUTBOUND] Providers notified about cancellation for offer {oferta.Destino.Nombre}.");
            // No devuelve nada (Task).
        }
    }
}
