namespace Gu.Wpf.ValidationScope;

using System;
using System.Windows.Markup;

/// <summary>
/// Markup extension for a convenient way to provide a list of types.
/// </summary>
[MarkupExtensionReturnType(typeof(InputTypeCollection))]
public partial class InputTypesExtension : MarkupExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputTypesExtension"/> class.
    /// </summary>
    public InputTypesExtension()
    {
    }

    private InputTypesExtension(Type[] types)
    {
        this.Types.AddRange(types);
    }

    /// <summary>
    /// Gets or sets the types to track validation for.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
    public InputTypeCollection Types { get; set; } = new InputTypeCollection();
#pragma warning restore CA2227 // Collection properties should be read only

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.Types;
    }
}
