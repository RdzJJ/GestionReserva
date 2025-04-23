namespace GestionReserva.Core.Interfaces
{
    // Interfaz genérica para repositorios (opcional).
    // Define un contrato básico y expone la Unidad de Trabajo.
    public interface IRepository<T> where T : Aggregates.ReservaAggregate.AggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        // Podrían añadirse métodos comunes como Add, Update, etc. si se desea.
    }
}