using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class UpdateCommunityRequestValidator : AbstractValidator<UpdateCommunityRequest>
{
    public UpdateCommunityRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(1000).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(ValidationMessages.Required);
    }
}