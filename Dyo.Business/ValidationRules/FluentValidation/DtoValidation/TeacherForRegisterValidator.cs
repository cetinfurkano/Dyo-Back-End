using Dyo.Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyo.Business.ValidationRules.FluentValidation.DtoValidation
{
    public class TeacherForRegisterValidator:AbstractValidator<TeacherForRegisterDto>
    {
        public TeacherForRegisterValidator()
        {
            var regexForLetter = new Regex(@"^[a-zA-ZğüşöçİĞÜŞÖÇ]+$");
            var regexForEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            var regexForPassword = 
                new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\\.])[A-Za-z\d@$!%*?&\\.]{8,}$");

            var regexForPhone = new Regex(@"^[0-9]{10}$");

            RuleFor(t => t.FirstName).NotEmpty().WithMessage("İsim boş olamaz!");
            RuleFor(t => t.FirstName).Cascade(CascadeMode.Stop).Matches(regexForLetter).WithMessage("İsim veya soyisim sadece harflerden oluşmalıdır!");

            RuleFor(t => t.LastName).NotEmpty().WithMessage("Soyisim boş olamaz!"); 
            RuleFor(t => t.LastName).Cascade(CascadeMode.Stop).Matches(regexForLetter).WithMessage("İsim veya soyisim sadece harflerden oluşmalıdır!");

            RuleFor(t => t.Email).NotEmpty();
            RuleFor(t => t.Email).Cascade(CascadeMode.Stop).Matches(regexForEmail).WithMessage("Gerçek bir eposta adresi girin");

            RuleFor(t => t.Password).NotEmpty().WithMessage("Şifre boş olamaz!");
            RuleFor(t => t.Password).Cascade(CascadeMode.Stop).Matches(regexForPassword).WithMessage("Şifre uygun formatta değil");

            RuleFor(t => t.PhoneNumber).NotEmpty().WithMessage("Telefon numarası boş olamaz!");
            RuleFor(t => t.PhoneNumber).Cascade(CascadeMode.Stop).Matches(regexForPhone).WithMessage("10 haneli telefon numarası girin(Başında 0 olmadan)");

            RuleFor(t => t.DistributorId).NotEmpty().WithMessage("Bir distribütör ID bilgisi girmek zorundasınız.");

            RuleFor(t => t.Address).SetValidator(new AddressValidator());
            
        }

    }
}
