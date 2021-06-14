using _2_NewbeExpressionTest.Impl;
using _2_NewbeExpressionTest.Interfaces;
using Autofac;

namespace _2_NewbeExpressionTest
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ValidatorFactory>()
                .As<IValidatorFactory>()
                .SingleInstance();

            builder.RegisterType<IntRangPropertyValidatorFactory>()
                .As<IPropertyValidatorFactory>()
                .SingleInstance();

            builder.RegisterType<StringRequiredPropertyValidatorFactory>()
                .As<IPropertyValidatorFactory>()
                .SingleInstance();

            builder.RegisterType<StringLengthPropertyValidatorFactory>()
                .As<IPropertyValidatorFactory>()
                .SingleInstance();
        }
    }
}