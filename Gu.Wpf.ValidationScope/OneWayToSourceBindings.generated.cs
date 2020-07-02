#pragma warning disable RS0041 // Public members should not use oblivious types
namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class OneWayToSourceBindings : FrameworkElement
    {
        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register(
            nameof(HasError),
            typeof(bool),
            typeof(OneWayToSourceBindings),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ErrorsProperty = DependencyProperty.Register(
            nameof(Errors),
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(OneWayToSourceBindings),
            new FrameworkPropertyMetadata(default(ReadOnlyObservableCollection<ValidationError>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register(
            nameof(Node),
            typeof(Node),
            typeof(OneWayToSourceBindings),
            new FrameworkPropertyMetadata(default(Node), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        internal static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",
            typeof(UIElement),
            typeof(OneWayToSourceBindings),
            new PropertyMetadata(default(UIElement), OnElementChanged));

        private static readonly DependencyProperty HasErrorProxyProperty = DependencyProperty.RegisterAttached(
            "HasErrorProxy",
            typeof(bool),
            typeof(OneWayToSourceBindings),
            new PropertyMetadata(default(bool), OnHasErrorProxyChanged));

        private static readonly DependencyProperty ErrorsProxyProperty = DependencyProperty.RegisterAttached(
            "ErrorsProxy",
            typeof(ReadOnlyObservableCollection<ValidationError>),
            typeof(OneWayToSourceBindings),
            new PropertyMetadata(default(ReadOnlyObservableCollection<ValidationError>), OnErrorsProxyChanged));

        private static readonly DependencyProperty NodeProxyProperty = DependencyProperty.RegisterAttached(
            "NodeProxy",
            typeof(Node),
            typeof(OneWayToSourceBindings),
            new PropertyMetadata(default(Node), OnNodeProxyChanged));

        public bool HasError
        {
            get => (bool)this.GetValue(HasErrorProperty);
            set => this.SetValue(HasErrorProperty, value);
        }

        public ReadOnlyObservableCollection<ValidationError> Errors
        {
            get => (ReadOnlyObservableCollection<ValidationError>)this.GetValue(ErrorsProperty);
            set => this.SetValue(ErrorsProperty, value);
        }

        public Node Node
        {
            get => (Node)this.GetValue(NodeProperty);
            set => this.SetValue(NodeProperty, value);
        }

        private static void OnHasErrorProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(HasErrorProperty, e.NewValue);
        }

        private static void OnErrorsProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(ErrorsProperty, e.NewValue);
        }

        private static void OnNodeProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(NodeProperty, e.NewValue);
        }

        private static void OnElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                BindingOperations.ClearBinding(d, DataContextProperty);
                BindingOperations.ClearBinding(d, HasErrorProxyProperty);
                BindingOperations.ClearBinding(d, ErrorsProxyProperty);
                BindingOperations.ClearBinding(d, NodeProxyProperty);
            }
            else
            {
                d.Bind(DataContextProperty)
                 .OneWayTo(e.NewValue, DataContextProperty);

                d.Bind(HasErrorProxyProperty)
                 .OneWayTo(e.NewValue, Scope.HasErrorProperty);

                d.Bind(ErrorsProxyProperty)
                 .OneWayTo(e.NewValue, Scope.ErrorsProperty);

                d.Bind(NodeProxyProperty)
                 .OneWayTo(e.NewValue, Scope.NodeProperty);
            }
        }
    }
}
