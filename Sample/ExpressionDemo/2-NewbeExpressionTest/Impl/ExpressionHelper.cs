using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class ExpressionHelper
    {
        public static Expression CreateCheckerExpression(Type valueType, Func<Expression, Expression> checkFuncArg, Func<Expression, Expression> errorMsgFuncArg)
        {
            // 构建校验入参
            var nameExp = Expression.Parameter(typeof(string), "name");
            var valueExp = Expression.Parameter(valueType, "value");

            // 构建校验表达式
            var inputExp = Expression.Parameter(typeof(CreateClaptrapInput), "input");
            var checkFuncBody = checkFuncArg.Invoke(inputExp);
            var checkExp = Expression.Invoke(checkFuncBody, valueExp);

            // 构建错误信息表达式
            var errorMsgFunc = errorMsgFuncArg.Invoke(inputExp);
            var errorMsgExp = Expression.Invoke(errorMsgFunc);

            // 构建返参实例化表达式
            var errorResultExp = Expression.Call(typeof(ValidateResult), nameof(ValidateResult.Error),
                Array.Empty<Type>(), errorMsgExp);
            var okResultExp = Expression.Call(typeof(ValidateResult), nameof(ValidateResult.Ok), Array.Empty<Type>());

            // 构建返回结果变量
            var resultExp = Expression.Variable(typeof(ValidateResult), "result");

        }

        /// <summary>
        /// 构建校验表达式
        /// </summary>
        /// <param name="valueType">值类型</param>
        /// <param name="checkFunc">校验Lambda</param>
        /// <param name="errorMsgFunc">构建错误信息Lambda</param>
        /// <returns></returns>
        public static Expression CreateCheckerExpression(Type valueType, Expression checkFunc, Expression errorMsgFunc) =>
            CreateCheckerExpression(valueType,
                inputExp => checkFunc,// 此处构建的func入参inputExp并没有在checkFunc中使用，仅为了兼顾 CreateCheckerExpression 的通用性
                inputExp => errorMsgFunc);
    }
}
