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
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            var propertyInfo = input.PropertyInfo;
            if (propertyInfo.GetCustomAttribute<RequiredAttribute>() != null)
            {
                Expression<Func<string, bool>> checkFunc = value => string.IsNullOrEmpty(value);
                Expression<Func<string, string>> errorMsgFunc = name => $"{name} 值不能为空";
                var checkExp = ExpressionHelper.CreateCheckerExpression(typeof(string), checkFunc, errorMsgFunc);
                yield return ExpressionHelper.CreateValidateExpression(input, checkExp);
            }
        }
    }
}