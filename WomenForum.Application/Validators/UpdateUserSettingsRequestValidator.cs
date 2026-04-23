using FluentValidation;
using WomenForum.Models.Requests;
using WomenForum.Domain.Enums;

namespace WomenForum.Validators;

public class UpdateUserSettingsRequestValidator : AbstractValidator<UpdateUserSettingsRequest>
{
    public UpdateUserSettingsRequestValidator()
    {
        RuleFor(x => x.Theme)
            .IsInEnum().WithMessage(ValidationMessages.InvalidValue);
    }
}