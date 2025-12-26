using FluentValidation;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

namespace Restaurants.Application.Restaurants.Validators;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(dto => dto)
            .Must(dto => dto.Name != null || dto.Description != null || dto.HasDelivery != null)
            .WithMessage("At least one field must be provided.");

        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 100)
            .When(dto => dto.Name != null);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .When(dto => dto.Description != null);
    }
}
