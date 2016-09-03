namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ValidationErrorFactory : ValidationError
    {
        private static readonly List<Tuple<UIElement, ValidationErrorFactory>> Cache = new List<Tuple<UIElement, ValidationErrorFactory>>();

        private ValidationErrorFactory(object bindingInError)
            : base(TestValidationRule.Default, bindingInError)
        {
        }

        public static ValidationErrorFactory GetFor(BindingExpression expression)
        {
            var source = (UIElement)expression.ParentBinding.Source;
            var match = Cache.SingleOrDefault(x => ReferenceEquals(x.Item1, source));
            if (match != null)
            {
                return match.Item2;
            }

            var error = new ValidationErrorFactory(expression.ParentBinding);
            Cache.Add(Tuple.Create(source, error));

            return error;
        }

        public static ValidationErrorFactory Create()
        {
            return new ValidationErrorFactory(new Binding());
        }

        public static ValidationErrorFactory GetFor(UIElement element)
        {
            return Cache.SingleOrDefault(x => ReferenceEquals(x.Item1, element))?.Item2;
        }
    }
}