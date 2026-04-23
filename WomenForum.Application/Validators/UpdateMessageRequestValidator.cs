using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class UpdateMessageRequestValidator : AbstractValidator<UpdateMessageRequest>
{
    public UpdateMessageRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(2000).WithMessage(ValidationMessages.MaxLength);
    }
}