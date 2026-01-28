using FluentValidation;
using RealEstate.Business.DTOs;

namespace RealEstate.Business.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad gereklidir")
            .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olmalıdır");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad gereklidir")
            .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olmalıdır");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi gereklidir")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gereklidir")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Şifre tekrarı gereklidir")
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+90|0)?[0-9]{10}$").WithMessage("Geçerli bir telefon numarası giriniz")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Rol gereklidir")
            .Must(x => x == "User" || x == "Agent" || x == "Admin")
            .WithMessage("Geçerli bir rol seçiniz (User, Agent, Admin)");
    }
}