using System;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Interfaces
{
    public interface IValidatorFactory
    {
        Func<object, ValidateResult> GetValidator(Type type);
    }
}