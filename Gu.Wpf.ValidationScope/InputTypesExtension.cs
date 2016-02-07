namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(InputTypeCollection))]
    public class InputTypesExtension : MarkupExtension
    {
        public InputTypesExtension()
        {
        }

        public InputTypesExtension(Type t1)
            : this(new[] { t1 })
        {
        }

        public InputTypesExtension(Type t1, Type t2)
            : this(new[] { t1, t2 })
        {
        }


        public InputTypesExtension(Type t1, Type t2, Type t3)
            : this(new[] { t1, t2, t3 })
        {
        }

        public InputTypesExtension(params Type[] types)
        {
            this.Types.AddRange(types);
        }

        public InputTypeCollection Types { get; set; } = new InputTypeCollection();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Types;
        }
    }
}