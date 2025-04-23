using Core.Aggregates;

namespace Core.Interfaces
{
    public interface IReservaRepository
    {
        Reserva? GetById(int id);
        void Add(Reserva reserva);
        void Update(Reserva reserva);
    }
}