using FluentValidation;
using WomenForum.Models.Requests;

namespace WomenForum.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Username) || !string.IsNullOrEmpty(x.Email))
            .WithMessage("Укажите имя пользователя или email.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Укажите пароль.");
    }
}