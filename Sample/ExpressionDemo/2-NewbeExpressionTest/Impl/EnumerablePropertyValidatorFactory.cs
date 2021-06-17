using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using _2_NewbeExpressionTest.Interfaces;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class EnumerablePropertyValidatorFactory : IPropertyValidatorFactory
    {
        public IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input)
        {
            var propertyType = input.PropertyInfo.PropertyType;

            // 源于string的特殊性，其本身就是实现了IEnumerable，但并不纳入集合校验的范围，故排除
            if (propertyType == typeof(string))
                yield break;
            // 获取属性继承的接口，并将自己纳入其中
            var interfaces = propertyType.GetInterfaces().Concat(new[] { propertyType });
            // 获取其继承接口或本身类型Name为"IEnumerable`1"的类型信息，为空则说明属性不为集合
            var type = interfaces.FirstOrDefault(x => x.Name.Equals("IEnumerable`1"));
            if (type == null)
                yield break;

            yield return CreateValidateAtLeastOneElementExpression(input, type);
        }

        /// <summary>
        /// 构建校验Expression
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private Expression CreateValidateAtLeastOneElementExpression(CreatePropertyValidatorInput input, Type type)
        {
            var valueExp = Expression.Parameter(type, "value");
            // 构建调用Enumerable.Any()函数的Expression 
            var anyExp = Expression.Call(typeof(Enumerable), nameof(Enumerable.Any), new[]
                {
                    type.GenericTypeArguments[0]// 获取第一个元素的Type
                }, valueExp);
            var isAnyExp = Expression.IsFalse(anyExp);
            // 构建指定类型的 System.Func,返回值为bool
            var itemTypeFunc = Expression.GetFuncType(type, typeof(bool));
            var checkFunc = Expression.Lambda(itemTypeFunc, isAnyExp, valueExp);
            Expression<Func<string, string>> errorMsgFunc = name => $"{name} 集合必须包含元素";

            var checkExp = ExpressionHelper.CreateCheckerExpression(type, checkFunc, errorMsgFunc);
            return ExpressionHelper.CreateValidateExpression(input, checkExp);

        }
    }
}