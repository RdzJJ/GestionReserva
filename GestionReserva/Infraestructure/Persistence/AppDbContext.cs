using Microsoft.EntityFrameworkCore;
using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.Interfaces; // Para IUnitOfWork
using System.Reflection; // Para ApplyConfigurationsFromAssembly
using System.Threading.Tasks;
using System.Threading;
using MediatR; // Para despachar eventos de dominio
using System.Linq;

namespace GestionReserva.Infrastructure.Persistence
{
    // Representa la sesi�n con la base de datos y act�a como Unit of Work.
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator? _mediator; // Opcional: Para despachar eventos de dominio.

        // DbSet para la Ra�z del Agregado principal. Las entidades contenidas se acceden a trav�s de Reserva.
        public DbSet<Reserva> Reservas { get; set; }

        // Constructor usado por Inyecci�n de Dependencias. Recibe opciones y opcionalmente MediatR.
        public AppDbContext(DbContextOptions<AppDbContext> options, IMediator? mediator = null) : base(options)
        {
            _mediator = mediator;
        }

        // Configura el modelo de datos usando las clases de configuraci�n Fluent API.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Aplica todas las configuraciones (IEntityTypeConfiguration) del ensamblado actual.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // Implementaci�n de IUnitOfWork para manejo expl�cito de transacciones.
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default) => await Database.BeginTransactionAsync(cancellationToken);
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default) => await Database.CommitTransactionAsync(cancellationToken);
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => await Database.RollbackTransactionAsync(cancellationToken);

        // Sobrescritura de SaveChangesAsync para despachar eventos de dominio ANTES de guardar.
        // Este m�todo es preferible para asegurar que los eventos se despachan y guardan at�micamente.
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            if (_mediator != null)
            {
                await DispatchDomainEventsAsync(_mediator); // Despacha eventos acumulados.
            }
            // Guarda los cambios en la base de datos.
            var result = await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        // M�todo privado para encontrar y despachar eventos de dominio usando MediatR.
        private async Task DispatchDomainEventsAsync(IMediator mediator)
        {
            // Encuentra todas las entidades AggregateRoot que tienen eventos pendientes.
            var domainEntities = this.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            // Obtiene todos los eventos pendientes.
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // Limpia los eventos de las entidades para evitar re-despacho.
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            // Publica cada evento usando MediatR.
            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }

        // Sobrescritura del SaveChangesAsync est�ndar para asegurar que los eventos se despachen
        // incluso si se llama directamente a este m�todo en lugar de SaveEntitiesAsync.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_mediator != null) { await DispatchDomainEventsAsync(_mediator); }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}