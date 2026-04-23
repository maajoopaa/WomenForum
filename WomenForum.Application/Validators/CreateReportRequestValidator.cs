using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class CreateReportRequestValidator : AbstractValidator<CreateReportRequest>
{
    public CreateReportRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(2000).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x)
            .Must(x =>
                x.PostId.HasValue ||
                x.UserId.HasValue ||
                x.CommunityId.HasValue ||
                x.ThreadId.HasValue)
            .WithMessage("Для жалобы необходимо указать как минимум один целевой показатель.");
    }
}