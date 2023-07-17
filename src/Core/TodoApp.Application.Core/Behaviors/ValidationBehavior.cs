using FluentValidation;
using MediatR;
using TodoApp.Application.Core.Exceptions;

namespace TodoApp.Application.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationTasks = _validators.Select(validator => validator.ValidateAsync(request, cancellationToken));

        var validationResults = await Task.WhenAll(validationTasks).ConfigureAwait(false);

        var validationFailures = validationResults.SelectMany(x => x.Errors);

        if (!validationResults.Any())
        {
            return await next().ConfigureAwait(false);
        }

        var errors = validationFailures.Select(validationFailure => new ValidationFailedException.ValidationError(validationFailure.PropertyName, validationFailure.ErrorMessage));

        throw new ValidationFailedException(errors);
    }
}
