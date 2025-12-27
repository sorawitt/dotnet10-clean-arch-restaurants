using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dish => dish.RestaurantId)
            .GreaterThan(0)
            .WithMessage("RestaurantId must be greater than 0.");

        RuleFor(dish => dish.Name)
            .NotEmpty()
            .Length(2, 120);

        RuleFor(dish => dish.Description)
            .NotEmpty();

        RuleFor(dish => dish.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");

        RuleFor(dish => dish.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .When(dish => dish.KiloCalories.HasValue)
            .WithMessage("KiloCalories must be a non-negative number.");
    }
}
