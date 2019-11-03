// ReSharper disable UnusedMember.Local
namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public static class ScopeEvents
    {
#pragma warning disable SA1202 // Elements must be ordered by access
        public static readonly DependencyProperty TrackProperty = DependencyProperty.RegisterAttached(
            "Track",
            typeof(bool),
            typeof(ScopeEvents),
            new PropertyMetadata(false, OnTrackChanged));

        private static readonly DependencyPropertyKey EventsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "Events",
            typeof(ObservableCollection<object>),
            typeof(ScopeEvents),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EventsProperty = EventsPropertyKey.DependencyProperty;

        private static readonly DependencyProperty HasErrorProperty = DependencyProperty.RegisterAttached(
            "HasError",
            typeof(bool?),
            typeof(ScopeEvents),
            new PropertyMetadata(null, (d, e) => GetEvents(d).Add($"HasError: {e.NewValue}")));

        private static readonly DependencyProperty ErrorsProperty = DependencyProperty.RegisterAttached(
            "Errors",
            typeof(IEnumerable<ValidationError>),
            typeof(ScopeEvents),
            new PropertyMetadata(null, (d, e) => GetEvents(d).Add(((IEnumerable<ValidationError>)e.NewValue).ToArray())));

        /// <summary>Helper for setting <see cref="TrackProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="TrackProperty"/> on.</param>
        /// <param name="value">Track property value.</param>
        public static void SetTrack(this DependencyObject element, bool value)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            element.SetValue(TrackProperty, value);
        }

        /// <summary>Helper for getting <see cref="TrackProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="TrackProperty"/> from.</param>
        /// <returns>Track property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetTrack(this DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(TrackProperty);
        }

        private static void SetEvents(this DependencyObject element, ObservableCollection<object> value)
        {
            element.SetValue(EventsPropertyKey, value);
        }

        /// <summary>Helper for getting <see cref="EventsProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="EventsProperty"/> from.</param>
        /// <returns>Events property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static ObservableCollection<object> GetEvents(this DependencyObject element)
        {
            if (element is null)
            {
                throw new System.ArgumentNullException(nameof(element));
            }

            return (ObservableCollection<object>)element.GetValue(EventsProperty);
        }

#pragma warning restore SA1202 // Elements must be ordered by access

        private static void OnTrackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(EventsPropertyKey, new ObservableCollection<object>());
            _ = d.Bind(HasErrorProperty)
                 .OneWayTo(d, Scope.HasErrorProperty);
            _ = d.Bind(ErrorsProperty)
                 .OneWayTo(d, Scope.ErrorsProperty);
            Scope.AddErrorHandler(d, (o, args) => GetEvents((DependencyObject)o).Add(args));
        }
    }
}
