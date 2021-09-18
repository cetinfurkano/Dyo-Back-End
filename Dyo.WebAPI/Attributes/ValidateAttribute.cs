using Dyo.Core.CrossCuttingConcerns.Validation.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateAttribute : Attribute, IAsyncActionFilter
    {
        private readonly Type _validatorType;
        public ValidateAttribute(Type validatorType)
        {
            _validatorType = validatorType;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = context.ActionArguments.Where(arg => arg.Value.GetType() == entityType);
            foreach (var entity in entities)
            {
                var validationResult = await ValidatorTool.ValidateAsync(validator, entity.Value);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.Errors.FirstOrDefault().ErrorMessage);
                    return;
                }
            }
            await next();
            
        }

    }
}
