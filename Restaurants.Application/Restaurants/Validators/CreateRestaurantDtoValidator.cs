using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private static readonly HashSet<string> ValidCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "Italian",
        "Mexican",
        "Japanese",
        "Thai",
        "American",
        "Indian"
    };

    public CreateRestaurantDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty();

        RuleFor(dto => dto.Category)
            .NotEmpty()
            .Must(ValidCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .When(dto => !string.IsNullOrWhiteSpace(dto.ContactEmail))
            .WithMessage("Invalid email address.");

        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^\+?[0-9\s\-()]{7,}$")
            .When(dto => !string.IsNullOrWhiteSpace(dto.ContactNumber))
            .WithMessage("Invalid phone number.");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^[0-9]{5}$")
            .When(dto => !string.IsNullOrWhiteSpace(dto.PostalCode))
            .WithMessage("Invalid postal code.");
    }
}
