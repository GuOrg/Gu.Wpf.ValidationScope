// ReSharper disable UnusedMember.Local
namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    public static class ValidationEvents
    {
#pragma warning disable SA1202 // Elements must be ordered by access
        public static readonly DependencyProperty TrackProperty = DependencyProperty.RegisterAttached(
            "Track",
            typeof(bool),
            typeof(ValidationEvents),
            new PropertyMetadata(false, OnTrackChanged));

        private static readonly DependencyPropertyKey EventsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Events",
            typeof(ObservableCollection<object>),
            typeof(ValidationEvents),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EventsProperty = EventsPropertyKey.DependencyProperty;

        private static readonly DependencyProperty HasErrorProperty = DependencyProperty.RegisterAttached(
            "HasError",
            typeof(bool?),
            typeof(ValidationEvents),
            new PropertyMetadata(null, (d, e) => GetEvents(d).Add($"HasError: {e.NewValue}")));

        private static readonly DependencyProperty ErrorsProperty = DependencyProperty.RegisterAttached(
            "Errors",
            typeof(IEnumerable<ValidationError>),
            typeof(ValidationEvents),
            new PropertyMetadata(null, (d, e) => GetEvents(d).Add(e.NewValue)));

        public static void SetTrack(this DependencyObject element, bool value)
        {
            element.SetValue(TrackProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetTrack(this DependencyObject element)
        {
            return (bool)element.GetValue(TrackProperty);
        }

        private static void SetEvents(this DependencyObject element, ObservableCollection<object> value)
        {
            element.SetValue(EventsPropertyKey, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static ObservableCollection<object> GetEvents(this DependencyObject element)
        {
            return (ObservableCollection<object>)element.GetValue(EventsProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnTrackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(EventsPropertyKey, new ObservableCollection<object>());
            BindingHelper.Bind(d, HasErrorProperty)
                         .OneWayTo(d, Validation.HasErrorProperty);
            BindingHelper.Bind(d, ErrorsProperty)
                         .OneWayTo(d, Validation.ErrorsProperty);
            Validation.AddErrorHandler(d, (o, args) => GetEvents((DependencyObject)o).Add(args));
        }
    }
}