namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows.Markup;

    /// <summary>
    /// <see cref="MarkupExtension"/> for accessing <see cref="InputTypeCollection.Default"/> in XAML.
    /// </summary>
    [MarkupExtensionReturnType(typeof(InputTypeCollection))]
    public class DefaultInputTypesExtension : MarkupExtension
    {
        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return InputTypeCollection.Default;
        }
    }
}
