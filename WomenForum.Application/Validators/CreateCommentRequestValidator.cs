using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(1000).WithMessage(ValidationMessages.MaxLength);
    }
}