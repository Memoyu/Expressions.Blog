using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using _2_NewbeExpressionTest.Interfaces;
using _2_NewbeExpressionTest.Models;
using Autofac;
using FluentAssertions;
using NUnit.Framework;

namespace _2_NewbeExpressionTest
{
    public class PropertyValidationTest_3

    {
        private const int Count = 10_000;

        private IValidatorFactory _factory = null!;

        [SetUp]
        public void Init()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<ValidatorModule>();
                var container = builder.Build();
                _factory = container.Resolve<IValidatorFactory>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Test]
        public void Run()
        {
            for (int i = 0; i < Count; i++)
            {
                {
                    var input = new CreateClaptrapInput
                    {
                        Name = "33333",
                        NickName = "newbe36524",
                        Age = 18,
                        Scores = new int[] { 90 },
                        Subjects = new List<string> { "语文" }
                    };
                    var (isOk, errorMessage) = Validate(input);
                    isOk.Should().BeTrue();
                    errorMessage.Should().BeNullOrEmpty();
                }
            }
        }

        public ValidateResult Validate(CreateClaptrapInput input)
        {
            Debug.Assert(_factory != null, nameof(_factory) + " != null");
            var validator = _factory.GetValidator(typeof(CreateClaptrapInput));
            return validator.Invoke(input);
        }
    }
}
