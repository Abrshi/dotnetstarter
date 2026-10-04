using FluentValidation;

namespace starter.Application.Auth.Commands.Login;

public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Username)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Request.Password)
            .NotEmpty();
    }
}