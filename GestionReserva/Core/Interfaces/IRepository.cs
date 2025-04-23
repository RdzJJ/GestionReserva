namespace GestionReserva.Core.Interfaces
{
    // Interfaz gen�rica para repositorios (opcional).
    // Define un contrato b�sico y expone la Unidad de Trabajo.
    public interface IRepository<T> where T : Aggregates.ReservaAggregate.AggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        // Podr�an a�adirse m�todos comunes como Add, Update, etc. si se desea.
    }
}