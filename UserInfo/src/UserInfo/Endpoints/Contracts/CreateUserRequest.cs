using FluentValidation;

namespace UserInfo.Endpoints.Contracts;

 public sealed record CreateUserRequest(string FirstName,string LastName, string NationalCode);


public sealed class CreateUserRequestValiator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValiator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Length(2, 50)
            .WithMessage("First name must be between 2 and 50 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .Length(2, 50)
            .WithMessage("Last name must be between 2 and 50 characters.");
        RuleFor(x => x.NationalCode)
            .NotEmpty()
            .WithMessage("National code is required.")
            .Must(x => x.ToString().Length == 10)
            .WithMessage("National code must be 10 digits.");
    }
}



