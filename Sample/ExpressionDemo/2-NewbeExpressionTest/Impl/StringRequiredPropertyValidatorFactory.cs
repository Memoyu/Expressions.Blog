using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class StringRequiredPropertyValidatorFactory : PropertyValidatorFactoryBase<string>
    {
        private static Expression<Func<string, string, ValidateResult>> CreateValidateStringRequiredExp()
        {
            return (name, value) =>
                string.IsNullOrEmpty(value)
                    ? ValidateResult.Error($"missing {name}")
                    : ValidateResult.Ok();
        }

        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            var propertyInfo = input.PropertyInfo;
            if (propertyInfo.GetCustomAttribute<RequiredAttribute>() != null)
            {
                yield return CreateValidateExpression(input, CreateValidateStringRequiredExp());
            }
        }
    }
}