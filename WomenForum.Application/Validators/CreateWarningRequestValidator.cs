using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class CreateWarningRequestValidator : AbstractValidator<CreateWarningRequest>
{
    public CreateWarningRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage(ValidationMessages.Required);
    }
}