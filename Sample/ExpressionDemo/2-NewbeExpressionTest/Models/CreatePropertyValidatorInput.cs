using System;
using System.Linq.Expressions;
using System.Reflection;

namespace _2_NewbeExpressionTest.Models
{
    public class CreatePropertyValidatorInput
    {
        // 校验属性的实体类型
        public Type InputType { get; set; } = null!;

        // 校验属性所属的实体入参表达式
        public Expression InputExpression { get; set; } = null!;

        // 校验属性信息
        public PropertyInfo PropertyInfo { get; set; } = null!;

        // 校验表达式返回结果参数
        public ParameterExpression ResultExpression { get; set; } = null!;

        // 跳转标签，用于Return跳转
        public LabelTarget ReturnLabel { get; set; } = null!;
    }
}