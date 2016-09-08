////namespace Gu.Wpf.ValidationScope
////{
////    using System.Windows;
////    using System.Windows.Data;

////    public class OneWayToSourceBindings : FrameworkElement
////    {
////        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register(
////            nameof(HasError),
////            typeof(bool),
////            typeof(OneWayToSourceBindings),
////            new PropertyMetadata(default(bool)));

////        internal static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
////            "Element",
////            typeof(UIElement),
////            typeof(OneWayToSourceBindings),
////            new PropertyMetadata(default(UIElement), OnElementChanged));

////        private static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
////                "HasErrorProxy",
////                typeof(bool),
////                typeof(OneWayToSourceBindings),
////                new PropertyMetadata(default(bool), OnHasErrorProxyChanged));

////        public bool HasError
////        {
////            get { return (bool)this.GetValue(HasErrorProperty); }
////            set { this.SetValue(HasErrorProperty, value); }
////        }

////        private static void OnHasErrorProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
////        {
////            d.SetCurrentValue(HasErrorProperty, e.NewValue);
////        }

////        private static void OnElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
////        {
////            if (e.NewValue == null)
////            {
////                BindingOperations.ClearBinding(d, DataContextProperty);
////                BindingOperations.ClearBinding(d, HasErrorProxyProperty);
////            }
////            else
////            {
////                d.Bind(DataContextProperty)
////                 .OneWayTo(e.NewValue, DataContextProperty);

////                d.Bind(HasErrorProxyProperty)
////                 .OneWayTo(e.NewValue, Scope.HasErrorProperty);
////            }
////        }
////    }
////}
