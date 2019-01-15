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

#pragma warning disable CA2227 // Collection properties should be read only
        public InputTypeCollection Types { get; set; } = new InputTypeCollection();
#pragma warning restore CA2227 // Collection properties should be read only

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Types;
        }
    }
}
