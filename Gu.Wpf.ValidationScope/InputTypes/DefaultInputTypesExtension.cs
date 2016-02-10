namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(InputTypeCollection))]
    public class DefaultInputTypesExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return InputTypeCollection.Default;
        }
    }
}