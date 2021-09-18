using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Core.CrossCuttingConcerns.Validation.FluentValidation
{
    public class ValidatorTool
    {
        public static async Task<ValidationResult> ValidateAsync(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);
            var result = await validator.ValidateAsync(context);
            return result;

        }
    }
}
