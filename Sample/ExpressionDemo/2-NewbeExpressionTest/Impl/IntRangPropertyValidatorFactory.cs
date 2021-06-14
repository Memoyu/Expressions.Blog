using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class IntRangPropertyValidatorFactory : PropertyValidatorFactoryBase<int>
    {
        private static Expression<Func<string, int, ValidateResult>> CreateValidateIntRangExp(int min, int max)
        {
            return (name, value) =>
                value < min && value < max
                    ? ValidateResult.Error($"{name}字段的值应在[{min},{max}]范围内")
                    : ValidateResult.Ok();
        }

        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            var propertyInfo = input.PropertyInfo;
            var rangAttr = propertyInfo.GetCustomAttribute<RangeAttribute>();
            if (rangAttr != null)
            {
                yield return CreateValidateExpression(input, CreateValidateIntRangExp((int)rangAttr.Minimum, (int)rangAttr.Maximum));
            }
        }
    }
}