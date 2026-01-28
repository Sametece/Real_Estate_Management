using FluentValidation;
using RealEstate.Business.DTOs;

namespace RealEstate.Business.Validators;

public class PropertyCreateDtoValidator : AbstractValidator<EPropertyCreateDto>
{
    public PropertyCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık gereklidir")
            .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır")
            .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olmalıdır");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama gereklidir")
            .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır")
            .MaximumLength(5000).WithMessage("Açıklama en fazla 5000 karakter olmalıdır");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(999999999).WithMessage("Fiyat en fazla 999.999.999 olabilir");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adres gereklidir")
            .MinimumLength(5).WithMessage("Adres en az 5 karakter olmalıdır")
            .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olmalıdır");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Şehir gereklidir")
            .MinimumLength(2).WithMessage("Şehir en az 2 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Şehir en fazla 100 karakter olmalıdır");

        RuleFor(x => x.District)
            .MaximumLength(100).WithMessage("İlçe en fazla 100 karakter olmalıdır")
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.Rooms)
            .GreaterThan(0).WithMessage("Oda sayısı 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(20).WithMessage("Oda sayısı en fazla 20 olabilir");

        RuleFor(x => x.Bathrooms)
            .GreaterThan(0).WithMessage("Banyo sayısı 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(10).WithMessage("Banyo sayısı en fazla 10 olabilir");

        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("Alan 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(100000).WithMessage("Alan en fazla 100.000 metrekare olabilir");

        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo(-10).WithMessage("Kat numarası en az -10 olabilir")
            .LessThanOrEqualTo(100).WithMessage("Kat numarası en fazla 100 olabilir");

        RuleFor(x => x.TotalFloors)
            .GreaterThan(0).WithMessage("Toplam kat sayısı 0'dan büyük olmalıdır")
            .LessThanOrEqualTo(200).WithMessage("Toplam kat sayısı en fazla 200 olabilir")
            .When(x => x.TotalFloors.HasValue);

        RuleFor(x => x.BuildYear)
            .GreaterThanOrEqualTo(1900).WithMessage("Yapım yılı en az 1900 olabilir")
            .LessThanOrEqualTo(2100).WithMessage("Yapım yılı en fazla 2100 olabilir");

        RuleFor(x => x.PropertyTypeId)
            .GreaterThan(0).WithMessage("Geçerli bir emlak tipi seçiniz");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Geçerli bir durum seçiniz");
    }
}