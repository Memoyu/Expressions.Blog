using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class StringLengthPropertyValidatorFactory : PropertyValidatorFactoryBase<string>
    {

        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            var propertyInfo = input.PropertyInfo;
            // 获取属性的MinLengthAttribute
            var minlengthAttribute = propertyInfo.GetCustomAttribute<MinLengthAttribute>();
            if (minlengthAttribute != null)
            {
                // 获取最小长度
                var minLength = minlengthAttribute.Length;
                // 构建校验属性值长度LambdaExpression
                Expression<Func<string, bool>> checkFunc = value => string.IsNullOrEmpty(value) || value.Length < minLength;
                // 构建错误信息LambdaExpression
                Expression<Func<string, string>> errorMsgFunc = name => $"{name} 值长度应大于 {minLength}";
                // 组装具体校验过程Expression
                var checkExp = ExpressionHelper.CreateCheckerExpression(typeof(string), checkFunc, errorMsgFunc);
                // 构建校验Expression
                yield return ExpressionHelper.CreateValidateExpression(input, checkExp);
            }

            var maxlengthAttribute = propertyInfo.GetCustomAttribute<MaxLengthAttribute>();
            if (maxlengthAttribute != null)
            {
                var maxLength = maxlengthAttribute.Length;
                Expression<Func<string, bool>> checkFunc = value => !string.IsNullOrEmpty(value) && value.Length > maxLength;
                Expression<Func<string, string>> errorMsgFunc = name => $"{name} 值长度应小于 {maxLength}";
                var checkExp = ExpressionHelper.CreateCheckerExpression(typeof(string), checkFunc, errorMsgFunc);
                yield return ExpressionHelper.CreateValidateExpression(input, checkExp);
            }
        }
    }
}