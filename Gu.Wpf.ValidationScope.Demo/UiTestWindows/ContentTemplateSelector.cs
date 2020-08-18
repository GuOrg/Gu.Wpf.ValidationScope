namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public class ContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate2 { get; set; } = null!;

        public DataTemplate DataTemplate1 { get; set; } = null!;

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                string text => text switch
                {
                    "int" => this.DataTemplate1,
                    "string" => this.DataTemplate2,
                    _ => base.SelectTemplate(item, container),
                },
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}
