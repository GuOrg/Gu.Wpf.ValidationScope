namespace Gu.Wpf.ValidationScope.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media;

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BoolToBrushConverter : MarkupExtension, IValueConverter
    {
        public SolidColorBrush WhenTrue { get; set; } = Brushes.Red;

        public SolidColorBrush WhenFalse { get; set; } = Brushes.Transparent;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, true) ? WhenTrue : WhenFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
