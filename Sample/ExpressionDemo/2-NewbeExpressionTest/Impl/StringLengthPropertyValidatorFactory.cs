﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class StringLengthPropertyValidatorFactory : PropertyValidatorFactoryBase<string>
    {
        private static Expression<Func<string, string, ValidateResult>> CreateValidateStringMinLengthExp(int minLength)
        {
            return (name, value) =>
                string.IsNullOrEmpty(value) || value.Length < minLength
                    ? ValidateResult.Error($"Length of {name} should be great than {minLength}")
                    : ValidateResult.Ok();
        }

        private static Expression<Func<string, string, ValidateResult>> CreateValidateStringMaxLengthExp(int maxLength)
        {
            return (name, value) =>
                !string.IsNullOrEmpty(value) && value.Length > maxLength
                    ? ValidateResult.Error($"Length of {name} should be less than {maxLength}")
                    : ValidateResult.Ok();
        }

        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            var propertyInfo = input.PropertyInfo;
            var minlengthAttribute = propertyInfo.GetCustomAttribute<MinLengthAttribute>();
            
            if (minlengthAttribute != null)
            {
                yield return CreateValidateExpression(input,
                    CreateValidateStringMinLengthExp(minlengthAttribute.Length));
            }

            var maxlengthAttribute = propertyInfo.GetCustomAttribute<MaxLengthAttribute>();
            if (maxlengthAttribute != null)
            {
                yield return CreateValidateExpression(input,
                    CreateValidateStringMaxLengthExp(maxlengthAttribute.Length));
            }
        }
    }
}