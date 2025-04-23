using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.ValueObjects;
using System.Threading;
using System.Threading.Tasks;
namespace GestionReserva.Core.Interfaces
{
    // Interfaz espec�fica para el repositorio del agregado Reserva.
    // Hereda de IRepository<Reserva> y a�ade m�todos espec�ficos.
    public interface IReservaRepository : IRepository<Reserva>
    {
        // Obtiene una reserva por su ID, cargando el agregado completo.
        Task<Reserva?> GetByIdAsync(ReservaId id, CancellationToken cancellationToken = default);
        // A�ade una nueva reserva al repositorio.
        Task AddAsync(Reserva reserva, CancellationToken cancellationToken = default);
        // Marca una reserva para ser actualizada (EF Core a menudo lo hace autom�ticamente).
        void Update(Reserva reserva);
        // Marca una reserva para ser eliminada (considerar borrado l�gico).
        void Remove(Reserva reserva);

        Task<IEnumerable<Reserva>> FindByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    }
}