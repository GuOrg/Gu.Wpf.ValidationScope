namespace Gu.Wpf.ValidationScope.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class PropertyChangedEventArgsComparer : GenericComparer<PropertyChangedEventArgs>
    {
        public static readonly PropertyChangedEventArgsComparer Default = new PropertyChangedEventArgsComparer();

        public override int Compare(PropertyChangedEventArgs x, PropertyChangedEventArgs y)
        {
            return Comparer<string>.Default.Compare(x.PropertyName, y.PropertyName);
        }
    }
}