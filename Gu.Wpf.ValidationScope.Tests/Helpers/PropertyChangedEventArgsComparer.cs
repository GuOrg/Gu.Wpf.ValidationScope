namespace Gu.Wpf.ValidationScope.Tests;

using System.Collections.Generic;
using System.ComponentModel;

internal sealed class PropertyChangedEventArgsComparer : GenericComparer<PropertyChangedEventArgs>
{
    internal static readonly PropertyChangedEventArgsComparer Default = new();

    protected override int Compare(PropertyChangedEventArgs x, PropertyChangedEventArgs y)
    {
        return Comparer<string>.Default.Compare(x.PropertyName, y.PropertyName);
    }
}
