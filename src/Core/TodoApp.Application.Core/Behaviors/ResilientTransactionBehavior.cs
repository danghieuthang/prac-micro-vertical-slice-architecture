using System.Windows.Input;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Core.Persistence;

namespace TodoApp.Application.Core.Behaviors;

public class ResilientTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest, ICommand
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ResilientTransactionBehavior(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var strategy = _unitOfWork.CreateExecuteStrategy();
        var response = await strategy.ExecuteAsync(async () => await TryExecuteStrategyAsync(next, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
        return response;
    }

    private async Task<TResponse> TryExecuteStrategyAsync(RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            var response = await next();

            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var domainEvents = _unitOfWork.ExtractDomainEventFromAggregates();

            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await _unitOfWork.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);

            var dispatchingTasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent));

            await Task.WhenAll(dispatchingTasks);

            return response;
        }
        catch
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}
