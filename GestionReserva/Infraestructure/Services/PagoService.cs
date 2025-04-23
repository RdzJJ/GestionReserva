using Core.Interfaces;

namespace Infrastructure.Services
{
    public class PagoService : IPagoService
    {
        public bool ProcesarPago(int reservaId, decimal monto, bool esPagoCompleto)
        {
            return true; // Simulación
        }
    }
}