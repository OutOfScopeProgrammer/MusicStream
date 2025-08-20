using FluentValidation;

namespace Music.API.Api.Controllers.IdentityController;

public class IdentityValidator : AbstractValidator<IdentityDto>
{
    public IdentityValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(i => i.PhoneNumber).MaximumLength(11).NotEmpty().NotNull().WithMessage("Phone number is required")
        .Must(ValidPhoneNumber).WithMessage("Phone number should start with \"09\" ");
        RuleFor(i => i.Password).NotEmpty().NotNull().MinimumLength(8)
        .WithMessage("Password is required")
        .Must(IsComplexEnough).WithMessage("password must contain number, upper case and lower case.");
    }

    private bool ValidPhoneNumber(string phoneNumber)
    {
        var startWithZero = phoneNumber.StartsWith("09");
        return startWithZero;
    }

    private bool IsComplexEnough(string password)
    {
        var upperCase = password.Any(char.IsUpper);
        var lowerCase = password.Any(char.IsLower);
        var digit = password.Any(char.IsDigit);
        return upperCase & lowerCase & digit;
    }
}

