namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class EventTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? StringTemplate { get; set; }

        public DataTemplate? ValidationErrorsCollectionTemplate { get; set; }

        public DataTemplate? EmptyValidationErrorsCollectionTemplate { get; set; }

        public DataTemplate? ValidationErrorEventArgsTemplate { get; set; }

        public DataTemplate? ScopeValidationErrorEventArgsTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is string)
            {
                return this.StringTemplate;
            }

            if (item is IEnumerable<ValidationError> collection)
            {
                if (collection.Any())
                {
                    return this.ValidationErrorsCollectionTemplate;
                }

                return this.EmptyValidationErrorsCollectionTemplate;
            }

            if (item is ValidationErrorEventArgs)
            {
                return this.ValidationErrorEventArgsTemplate;
            }

            if (item is ScopeValidationErrorEventArgs)
            {
                return this.ScopeValidationErrorEventArgsTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
