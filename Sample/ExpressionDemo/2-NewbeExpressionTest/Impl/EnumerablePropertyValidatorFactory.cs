using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            throw new NotImplementedException();
        }
    }
}