using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(500).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Logo)
            .MaximumLength(2048).WithMessage(ValidationMessages.MaxLength)
            .When(x => !string.IsNullOrEmpty(x.Logo));
    }
}