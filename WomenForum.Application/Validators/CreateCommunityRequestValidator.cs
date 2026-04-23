using FluentValidation;
using WomenForum.Models.Requests;
using WomenForum.Domain.Enums;

namespace WomenForum.Validators;

public class CreateCommunityRequestValidator : AbstractValidator<CreateCommunityRequest>
{
    public CreateCommunityRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(1000).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Avatar)
            .MaximumLength(2048).WithMessage(ValidationMessages.MaxLength)
            .When(x => !string.IsNullOrEmpty(x.Avatar));

        RuleFor(x => x.Visibility)
            .IsInEnum().WithMessage(ValidationMessages.InvalidValue);

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(ValidationMessages.Required);
    }
}