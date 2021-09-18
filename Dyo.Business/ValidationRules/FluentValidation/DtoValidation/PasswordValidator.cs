using Dyo.Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyo.Business.ValidationRules.FluentValidation.DtoValidation
{
    public class PasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public PasswordValidator()
        {
            var regexForPassword =
                new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&\\.])[A-Za-z\d@$!%*?&\\.]{8,}$");
            
            RuleFor(t => t.OldPassword).NotEmpty().WithMessage("Şifre boş olamaz!");
            RuleFor(t => t.OldPassword).Cascade(CascadeMode.Stop).Matches(regexForPassword).WithMessage("Şifre uygun formatta değil");

            RuleFor(t => t.NewPassword).NotEmpty().WithMessage("Şifre boş olamaz!");
            RuleFor(t => t.NewPassword).Cascade(CascadeMode.Stop).Matches(regexForPassword).WithMessage("Şifre uygun formatta değil");
        }
    }
}
