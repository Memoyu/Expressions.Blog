using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using _2_NewbeExpressionTest.Interfaces;
using _2_NewbeExpressionTest.Models;

namespace _2_NewbeExpressionTest.Impl
{
    public abstract class PropertyValidatorFactoryBase<TValue> : IPropertyValidatorFactory
    {
        public virtual IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.PropertyType != typeof(TValue))
            {
                return Enumerable.Empty<Expression>();
            }

            var expressionCore = CreateExpressionCore(input);
            return expressionCore;
        }

        protected abstract IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input);
    }
}