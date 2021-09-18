using Dyo.Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyo.Business.ValidationRules.FluentValidation.DtoValidation
{
    public class TeacherForLoginValidator : AbstractValidator<TeacherForLoginDto>
    {
        public TeacherForLoginValidator()
        {
            RuleFor(t => t.Email).NotEmpty().WithMessage("Email boş olamaz!");
            RuleFor(t => t.Email).Cascade(CascadeMode.Stop).
               Matches(new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")).WithMessage("Lütfen gerçek bir eposta adresi girin");
            //email

            RuleFor(t => t.Password).NotEmpty().WithMessage("Şifre boş olamaz!");
            RuleFor(t => t.Password).Cascade(CascadeMode.Stop)
                .Matches(new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\\.])[A-Za-z\d@$!%*?&\\.]{8,}$")).WithMessage("Şifre uygun formatta değil!");
            //Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
        }
    }
}
