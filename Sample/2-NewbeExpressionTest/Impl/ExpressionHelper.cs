using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public class ExpressionHelper
    {
        /// <summary>
        /// 构建校验表达式
        /// </summary>
        /// <param name="input"></param>
        /// <param name="checkFuncExp"></param>
        /// <returns></returns>
        public static Expression CreateValidateExpression(CreatePropertyValidatorInput input, Expression checkFuncExp)
        {
            // 获取input属性信息
            var inputPropertyInfo = input.PropertyInfo;
            var validateResultPropertyInfo = typeof(ValidateResult).GetProperty(nameof(ValidateResult.IsOk));
            Debug.Assert(validateResultPropertyInfo != null, nameof(validateResultPropertyInfo) + " != null");

            // 强制转换需要检验属性的实体类型
            var convertInputExp = Expression.Convert(input.InputExpression, input.InputType);
            // 构建成员字段名称表达式
            var nameExp = Expression.Constant(inputPropertyInfo.Name);
            // 构建获取成员字段的值表达式
            var propExp = Expression.Property(convertInputExp, inputPropertyInfo);

            // 构建 InvocationExpression（表示调用具有指定变量的lambda表达式）
            var validateInvocationExp = Expression.Invoke(checkFuncExp, nameExp, propExp, convertInputExp);

            // 执行校验，并将结果赋值到input中的ResultExpression
            var assignValidateResultExp = Expression.Assign(input.ResultExpression, validateInvocationExp);

            // 获取校验结果的IsOk属性值，并构建是否为False表达式
            var validateResultExp = Expression.Property(input.ResultExpression, validateResultPropertyInfo);
            var conditionExp = Expression.IsFalse(validateResultExp);//计算结果是否为False

            // 构建判断是否进行校验结果赋值表达式
            var ifThenExp =
                Expression.IfThen(conditionExp, Expression.Return(input.ReturnLabel, input.ResultExpression));

            var result = Expression.Block(new[] { input.ResultExpression }, assignValidateResultExp, ifThenExp);
            return result;
        }

        /// <summary>
        /// 构建实际执行校验并返回校验结果Lambda表达式
        /// </summary>
        /// <param name="valueType">校验值类型</param>
        /// <param name="checkFuncArg">实际校验执行的Func</param>
        /// <param name="errorMsgFuncArg">返回错误信息Func</param>
        /// <returns></returns>
        public static Expression CreateCheckerExpression(Type valueType, Func<Expression, Expression> checkFuncArg, Func<Expression, Expression> errorMsgFuncArg)
        {
            // 构建校验入参
            var nameExp = Expression.Parameter(typeof(string), "name");
            var valueExp = Expression.Parameter(valueType, "value");

            // 构建校验表达式
            var inputType = typeof(CreateClaptrapInput);
            var inputExp = Expression.Parameter(inputType, "input");
            var checkFuncBody = checkFuncArg.Invoke(inputExp);
            var checkExp = Expression.Invoke(checkFuncBody, valueExp);

            // 构建错误信息表达式
            var errorMsgFunc = errorMsgFuncArg.Invoke(inputExp);
            var errorMsgExp = Expression.Invoke(errorMsgFunc, nameExp);

            // 构建返参实例化表达式
            var errorResultExp = Expression.Call(typeof(ValidateResult), nameof(ValidateResult.Error), Array.Empty<Type>(), errorMsgExp);
            var okResultExp = Expression.Call(typeof(ValidateResult), nameof(ValidateResult.Ok), Array.Empty<Type>());

            // 构建返回结果变量表达式
            var resultExp = Expression.Variable(typeof(ValidateResult), "result");
            // 构建校验结果判断并赋值ValidateResult
            var judgeResultExp = Expression.IfThenElse(checkExp,
                Expression.Assign(resultExp, errorResultExp),
                Expression.Assign(resultExp, okResultExp));
            // 组装判断、赋值表达式
            var bodyExp = Expression.Block(new[] { resultExp }, judgeResultExp, resultExp);

            // 构建Func类型,格式Func<string, valueType, inputType, ValidateResult>
            var funcType = Expression.GetFuncType(typeof(string), valueType, inputType, typeof(ValidateResult));

            // 组装最终返回的Lambda表达式
            var finalExp = Expression.Lambda(funcType, bodyExp, nameExp, valueExp, inputExp);
            return finalExp;
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

        public static Expression CreateCheckerExpression(Type valueType, Expression checkFunc, Func<Expression, Expression> errorMsgFuncFactory) =>
            CreateCheckerExpression(valueType,
                inputExp => checkFunc,
                errorMsgFuncFactory);
    }
}
