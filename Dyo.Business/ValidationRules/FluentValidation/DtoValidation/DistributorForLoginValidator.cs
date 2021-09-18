using Dyo.Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyo.Business.ValidationRules.FluentValidation.DtoValidation
{
    public class DistributorForLoginValidator:AbstractValidator<DistributorForLoginDto>
    {
        public DistributorForLoginValidator()
        {
            var regexForEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            var regexForPassword =
                 new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\\.])[A-Za-z\d@$!%*?&\\.]{8,}$");

            RuleFor(d => d.Email).NotEmpty().WithMessage("Şifre boş olamaz!");
            RuleFor(d => d.Email).Cascade(CascadeMode.Stop).Matches(regexForEmail).WithMessage("Lütfen gerçek bir eposta adresi girin!");

            RuleFor(d => d.Password).NotEmpty().WithMessage("Şifre boş olamaz");
            RuleFor(d => d.Password).Cascade(CascadeMode.Stop).Matches(regexForPassword).WithMessage("Şifre uygun formatta değil!"); ;
        }
    }
}
