using Dyo.Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyo.Business.ValidationRules.FluentValidation.DtoValidation
{
    public class DistributorForRegisterValidator : AbstractValidator<DistributorForRegisterDto>
    {
        public DistributorForRegisterValidator()
        {
            var regexForLetter = new Regex(@"^[a-zA-ZğüşöçİĞÜŞÖÇ]+$");
            var regexForEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            var regexForPassword =
                new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\\.])[A-Za-z\d@$!%*?&\\.]{8,}$");

            var regexForPhone = new Regex(@"^[0-9]{10}$");

            RuleFor(d => d.Email).NotEmpty().WithMessage("Eposta adresi boş olamaz!");
            RuleFor(d => d.Email).Cascade(CascadeMode.Stop).Matches(regexForEmail).WithMessage("Lütfen gerçek bir eposta adresi girin!");

            RuleFor(d => d.FirstName).NotEmpty().WithMessage("İsim boş olamaz!"); ;
            RuleFor(d => d.FirstName).Cascade(CascadeMode.Stop).Matches(regexForLetter).WithMessage("İsim veya soyisim sadece harflerden oluşmalıdır!");

            RuleFor(d => d.LastName).NotEmpty().WithMessage("Soyisim boş olamaz"); ;
            RuleFor(d => d.LastName).Cascade(CascadeMode.Stop).Matches(regexForLetter).WithMessage("İsim veya soyisim sadece harflerden oluşmalıdır!"); ;

            RuleFor(d => d.PhoneNumber).NotEmpty();
            RuleFor(d => d.PhoneNumber).Cascade(CascadeMode.Stop).Matches(regexForPhone).WithMessage("10 haneli telefon numarası girin(Başında 0 olmadan)"); ;

            RuleFor(d => d.Password).NotEmpty();
            RuleFor(d => d.Password).Cascade(CascadeMode.Stop).Matches(regexForPassword);


        }
    }
}
