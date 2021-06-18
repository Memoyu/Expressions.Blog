using _2_NewbeExpressionTest.Attributes;
using _2_NewbeExpressionTest.Interfaces;
using _2_NewbeExpressionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _2_NewbeExpressionTest.Impl
{
    public class EnumRangPropertyValidatorFactory : IPropertyValidatorFactory
    {
        public IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input)
        {
            var type = input.PropertyInfo.PropertyType;
            var enumRangAttribute = input.PropertyInfo.GetCustomAttribute<EnumRangAttribute>();
            // 此处校验类型必须使用enum类型的BaseType
            if (type.BaseType != typeof(Enum) || enumRangAttribute == null)
            {
                yield break;
            }

            // 获取type枚举类型所有value
            var enumValues = Enum.GetValues(type).Cast<object>().ToList();

            var valueExp = Expression.Parameter(type, "value");
            var enumValuesExp = Expression.Constant(enumValues);
            var convertValueExp = Expression.Convert(valueExp, typeof(object));

            // 构建校验主体Exoression
            var isContainExp = Expression.Call(enumValuesExp, nameof(List<object>.Contains), Array.Empty<Type>(), convertValueExp);
            var isResultExp = Expression.IsFalse(isContainExp);
            var funcTypeExp = Expression.GetFuncType(type, typeof(bool));
            var checkFunc = Expression.Lambda(funcTypeExp, isResultExp, valueExp);

            // 构建错误消息Expression
            Expression ErrorMsgFuncFactory(Expression inputExp)
            {
                var nameExp = Expression.Parameter(typeof(string), "name");
                var msgFormatExp = Expression.Constant("{0} 值为 {1} 并不在{2}中");
                var enumValuesStr = string.Join(",", enumValues.Cast<Enum>().Select(x => x.ToString("D")));
                var bodyExp = Expression.Call(typeof(string),
                    nameof(string.Format),
                    Array.Empty<Type>(),
                    msgFormatExp,
                    Expression.Convert(nameExp, typeof(object)),
                    Expression.Convert(Expression.Constant(enumValuesStr), typeof(object)),
                    Expression.Convert(Expression.Property(inputExp, input.PropertyInfo), typeof(object)));
                var errorMessageFunc = Expression.Lambda<Func<string, string>>(bodyExp, nameExp);
                return errorMessageFunc;
            }
            yield return ExpressionHelper.CreateValidateExpression(input, ExpressionHelper.CreateCheckerExpression(type, checkFunc, ErrorMsgFuncFactory));
        }
    }
}
