using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(20).WithMessage(ValidationMessages.MaxLength)
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage(ValidationMessages.InvalidUsername);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MinimumLength(6).WithMessage(ValidationMessages.MinLength);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .Equal(x => x.Password)
            .WithMessage(ValidationMessages.PasswordsNotMatch);
        
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.UtcNow)
            .WithMessage(ValidationMessages.InvalidBirthDate);
    }
}