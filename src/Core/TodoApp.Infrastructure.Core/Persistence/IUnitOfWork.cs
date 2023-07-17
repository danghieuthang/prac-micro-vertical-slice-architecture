using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace TodoApp.Infrastructure.Core.Persistence;
public interface IUnitOfWork
{
    IEnumerable<INotification> ExtractDomainEventFromAggregates();
    IExecutionStrategy CreateExecuteStrategy();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollBackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
