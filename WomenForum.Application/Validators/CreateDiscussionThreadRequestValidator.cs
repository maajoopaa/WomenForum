using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class CreateDiscussionThreadRequestValidator : AbstractValidator<CreateDiscussionThreadRequest>
{
    public CreateDiscussionThreadRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(150).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(2000).WithMessage(ValidationMessages.MaxLength);
    }
}