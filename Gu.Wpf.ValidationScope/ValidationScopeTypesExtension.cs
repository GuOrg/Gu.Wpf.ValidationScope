namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(ValidationScopeTypes))]
    public class ValidationScopeTypesExtension : MarkupExtension
    {
        public ValidationScopeTypesExtension()
        {
        }

        public ValidationScopeTypesExtension(IEnumerable<Type> types)
        {
            this.Types.AddRange(types);
        }

        public ValidationScopeTypes Types { get; set; } = new ValidationScopeTypes();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Types;
        }
    }
}