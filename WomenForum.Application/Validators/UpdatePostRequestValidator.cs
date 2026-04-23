using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.HtmlContent)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(10000).WithMessage(ValidationMessages.MaxLength);
    }
}