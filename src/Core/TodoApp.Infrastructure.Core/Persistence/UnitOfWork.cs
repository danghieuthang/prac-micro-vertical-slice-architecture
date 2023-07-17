using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TodoApp.Domain.Core;

namespace TodoApp.Infrastructure.Core.Persistence;
public class UnitOfWork<TDBContext> : IUnitOfWork
    where TDBContext : DbContext
{
    private readonly TDBContext _context;

    public UnitOfWork(TDBContext dBContext)
    {
        _context = dBContext;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    public IExecutionStrategy CreateExecuteStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public IEnumerable<INotification> ExtractDomainEventFromAggregates()
    {
        var aggregates = _context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Any());

        var domainEvents = aggregates.SelectMany(entry => entry.Entity.DomainEvents).ToArray();

        foreach (var aggregate in aggregates)
        {
            aggregate.Entity.ClearDomainEvent();
        }

        return domainEvents;
    }

    public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
