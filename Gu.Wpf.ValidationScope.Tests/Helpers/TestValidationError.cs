namespace Gu.Wpf.ValidationScope.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class TestValidationError : ValidationError
    {
        private static readonly List<Tuple<UIElement, TestValidationError>> Cache = new List<Tuple<UIElement, TestValidationError>>();

        private TestValidationError(object bindingInError)
            : base(TestValidationRule.Default, bindingInError)
        {
        }

        public static TestValidationError GetFor(BindingExpression expression)
        {
            var source = (UIElement)expression.ParentBinding.Source;
            var match = Cache.SingleOrDefault(x => ReferenceEquals(x.Item1, source));
            if (match != null)
            {
                return match.Item2;
            }

            var error = new TestValidationError(expression.ParentBinding);
            Cache.Add(Tuple.Create(source, error));

            return error;
        }

        public static TestValidationError Create()
        {
            return new TestValidationError(new Binding());
        }

        public static TestValidationError GetFor(UIElement element)
        {
            return Cache.SingleOrDefault(x => ReferenceEquals(x.Item1, element))?.Item2;
        }
    }
}