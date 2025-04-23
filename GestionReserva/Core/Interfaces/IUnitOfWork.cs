using System;
using System.Threading;
using System.Threading.Tasks;
namespace GestionReserva.Core.Interfaces
{
    // Define el contrato para el patrón Unit of Work.
    // Coordina el guardado de cambios y maneja transacciones.
    public interface IUnitOfWork : IDisposable
    {
        // Guarda todos los cambios realizados en el contexto actual.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        // Inicia una transacción de base de datos.
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        // Confirma la transacción actual.
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        // Revierte la transacción actual.
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        // Guarda los cambios y despacha los eventos de dominio acumulados.
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}