using FluentValidation;
using RealEstate.Business.DTOs;

namespace RealEstate.Business.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi gereklidir")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gereklidir")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");
    }
}