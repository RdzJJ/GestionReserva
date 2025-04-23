namespace Core.Interfaces
{
    public interface IPagoService
    {
        bool ProcesarPago(int reservaId, decimal monto, bool esPagoCompleto);
    }
}