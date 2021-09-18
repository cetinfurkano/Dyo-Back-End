using Dyo.Entity.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyo.Business.ValidationRules.FluentValidation.DtoValidation
{
    public class CategoryForRegisterValidator:AbstractValidator<CategoryForRegisterDto>
    {
        public CategoryForRegisterValidator()
        {
            var regexForLetter = new Regex(@"^[a-zA-ZğüşöçİĞÜŞÖÇ]+$");
           
            RuleFor(c => c.CategoryName).Cascade(CascadeMode.Stop).Matches(regexForLetter).WithMessage("Kategori Adı sadece harflerden oluşmalıdır!");
        }
    }
}
