using Core.Interfaces;


namespace Infraestructure.Services
{
    public class PagoService : IPagoService
    {
        public bool ProcesarPago(int reservaID, decimal monto, bool esPagoCompleto)
        {
            return true; // solamente para hacer la simulación
        }
    }
}