namespace Gu.Wpf.ValidationScope
{
    using System.ComponentModel;
    using System.Windows;

    internal static class Is
    {
        private static readonly DependencyObject DependencyObject = new DependencyObject();

        public static bool DesignMode => DesignerProperties.GetIsInDesignMode(DependencyObject);
    }
}
