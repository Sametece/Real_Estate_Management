using FluentValidation;
using RealEstate.Business.DTOs;

namespace RealEstate.Business.Validators;

public class InquiryCreateDtoValidator : AbstractValidator<InquiryCreateDto>
{
    public InquiryCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad soyad gereklidir")
            .MinimumLength(2).WithMessage("Ad soyad en az 2 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olmalıdır");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi gereklidir")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesaj gereklidir")
            .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır")
            .MaximumLength(1000).WithMessage("Mesaj en fazla 1000 karakter olmalıdır");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("Geçerli bir emlak ilanı seçiniz");
    }
}