using Core.Entities;

namespace Core.Interfaces
{
    public interface IReservaRepository
    {
        void Add(Reserva reserva);
        void Update(Reserva reserva);
        Reserva GetById(int id);
    }
}