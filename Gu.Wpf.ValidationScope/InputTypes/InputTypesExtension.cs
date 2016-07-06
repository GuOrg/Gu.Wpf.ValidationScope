namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(InputTypeCollection))]
    public partial class InputTypesExtension : MarkupExtension
    {
        public InputTypesExtension()
        {
        }

        private InputTypesExtension(Type[] types)
        {
            this.Types.AddRange(types);
        }

        public InputTypeCollection Types { get; set; } = new InputTypeCollection();

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Types;
        }
    }
}