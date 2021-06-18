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
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            var propertyInfo = input.PropertyInfo;
            var rangAttr = propertyInfo.GetCustomAttribute<RangeAttribute>();
            if (rangAttr != null)
            {
                var min = (int)rangAttr.Minimum;
                var max = (int)rangAttr.Maximum;
                Expression<Func<int, bool>> checkFunc = value => value < min && value < max;
                Expression<Func<string, string>> errorMsgFunc = name => $"{name} 值应在[{min},{max}]范围内";
                var checkExp = ExpressionHelper.CreateCheckerExpression(typeof(int), checkFunc, errorMsgFunc);
                yield return ExpressionHelper.CreateValidateExpression(input, checkExp);
            }
        }
    }
}