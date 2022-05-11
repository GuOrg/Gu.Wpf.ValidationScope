namespace Gu.Wpf.ValidationScope.Demo
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(Visibility))]
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public static readonly NullToVisibilityConverter CollapsedWhenNull = new(Visibility.Collapsed);
        private readonly Visibility whenNull;

        private NullToVisibilityConverter(Visibility whenNull)
        {
            this.whenNull = whenNull;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? this.whenNull : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
