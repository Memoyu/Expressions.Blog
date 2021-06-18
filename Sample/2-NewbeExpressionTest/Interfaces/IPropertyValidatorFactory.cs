using System.Collections.Generic;
using System.Linq.Expressions;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Interfaces
{
    public interface IPropertyValidatorFactory
    {
        IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input);
    }
}