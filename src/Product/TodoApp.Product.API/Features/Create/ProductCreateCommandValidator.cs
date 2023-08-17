using FluentValidation;

namespace TodoApp.Product.API.Features.Create;

public class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(command => command.Price)
            .GreaterThan(decimal.Zero);

        RuleFor(command => command.Quantity)
            .GreaterThan(0);
    }
}
