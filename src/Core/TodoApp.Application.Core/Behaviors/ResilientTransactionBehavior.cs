using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Core;
using TodoApp.Infrastructure.Core.Persistence;

namespace TodoApp.Application.Core.Behaviors;

public class ResilientTransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork, IMediator mediator) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, ICommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var strategy = unitOfWork.CreateExecuteStrategy();
        var response = await strategy.ExecuteAsync(async () => await TryExecuteStrategyAsync(next, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
        return response;
    }

    private async Task<TResponse> TryExecuteStrategyAsync(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            var response = await next();

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var domainEvents = unitOfWork.ExtractDomainEventFromAggregates();

            await unitOfWork.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);

            var dispatchingTasks = domainEvents.Select(domainEvent => mediator.Publish(domainEvent));

            await Task.WhenAll(dispatchingTasks);

            return response;
        }
        catch
        {
            await unitOfWork.RollBackTransactionAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}
